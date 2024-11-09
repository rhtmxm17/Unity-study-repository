using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    
    private CharacterController controller;
    private InputAction moveInput;
    private InputAction jumpInput;

    private Vector3 moveVector;

    private float zeroVelocityJumpTime;
    private float FallBeginTime;

    private bool isGrounded = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        moveInput = playerInput.actions["Move"];
        jumpInput = playerInput.actions["Jump"];
        jumpInput.started += OnJumpInput;

        zeroVelocityJumpTime = jumpSpeed / -Physics.gravity.y;
    }


    //// 사용자 입력을 처리하는 컴포넌트를 명확히 분리한다면 아예 제거/비활성화하는 것도 유효
    //private void Start()
    //{
    //    if (!HasStateAuthority)
    //    {
    //        Debug.Log("소유권이 없는 컨트롤 파괴");
    //        Destroy(this);
    //    }
    //}

    // 해당 클라이언트에서만 적용되는 Update 주기
    private void Update()
    {
        //// 소유권이 없는 NetworkBehaviour의 Update도 호출됨. 유의
        //Debug.Log($"Update 소유권: {HasStateAuthority}");

        if (! HasStateAuthority)
            return;

        Vector3 moveAxisX = Camera.main.transform.right;
        Vector3 moveAxisY = Camera.main.transform.forward;

        moveAxisX.y = 0f;
        moveAxisY.y = 0f;

        moveAxisX.Normalize();
        moveAxisY.Normalize();

        Vector2 input = moveInput.ReadValue<Vector2>();
        moveVector = input.x * moveAxisX + input.y * moveAxisY;
    }

    private void OnDrawGizmos()
    {
        // isGrounded 체크 Gizmo
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * 0.2f);
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        if (false == isGrounded)
            return;

        isGrounded = false;
        FallBeginTime = Runner.SimulationTime + zeroVelocityJumpTime; // 낙하 속도가 0이 되는 시간을 저장
    }

    // 네트워크 통신 주기마다 Network Object에 의해 호출됨
    // 네트워크 사이클 상에서 Update처럼 동작
    public override void FixedUpdateNetwork()
    {
        //// 항상 true 출력됨. 소유권을 갖는 객체의 FixedUpdateNetwork만 호출되는듯?
        //Debug.Log($"FixedUpdateNetwork 소유권: {HasStateAuthority}");

        // 그래도 매뉴얼에서 예외처리를 하고있으니 따르는게 좋을 것 같다
        if (! HasStateAuthority)
            return;

        Debug.Log($"isGrounded: {isGrounded} | SimulationTime: {Runner.SimulationTime} | SimulationDeltaTime: {Runner.DeltaTime} | FallBeginTime: {FallBeginTime}");

        // 점프 또는 낙하 시간으로부터 y축 속도를 계산하는 방식을 사용해보았다
        // 예상 특징: 천장에 막혀도 상승 시간은 보장되는 조작감(플랫포머에서 자주 보는)
        // 예상 단점: 공중에서 다른 요소로 인해 속도가 변하는 상황의 처리가 복잡해짐

        if (moveVector != Vector3.zero)
            transform.forward = moveVector;

        if (isGrounded)
        {
            controller.Move(Runner.DeltaTime * moveSpeed * moveVector);
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
            if (false == isGrounded)
            {
                FallBeginTime = Runner.SimulationTime; // 낙하 속도가 0이 되는 시간 == 현재
            }
        }
        else
        {
            Vector3 velocity = moveSpeed * moveVector;
            velocity.y = Physics.gravity.y * (Runner.SimulationTime - FallBeginTime);
            controller.Move(Runner.DeltaTime * velocity);

            // 점프 시작시 isGrounded가 되면 안되므로 낙하중일 경우에만 isGrounded 검사
            if (velocity.y <= 0f)
            {
                isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
            }
        }
    }

    // 네트워크 오브젝트 생성시 호출됨
    public override void Spawned()
    {
        // 소유권이 없는 NetworkBehaviour의 Spawned도 호출됨
        Debug.Log($"Spawned 소유권: {HasStateAuthority}");

        // 내 클라이언트에 다른 사람의 캐릭터가 나타났을 때에도 해야 할 처리

        if (! HasStateAuthority)
            return;

        // 내 캐릭터에 대해서만 해야 할 처리(예: 카메라 추적 대상 설정)
        if (Camera.main.TryGetComponent(out CameraController camControl))
        {
            camControl.Target = this.transform;
        }
    }

}

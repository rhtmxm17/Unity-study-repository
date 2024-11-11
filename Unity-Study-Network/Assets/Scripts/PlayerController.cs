using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] float baseMoveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] RayCastGun attackComponent;

    private CharacterController controller;
    private InputAction moveInput;
    private InputAction jumpInput;
    private InputAction shiftInput;
    private InputAction fireAction;

    private float moveSpeed;
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
        shiftInput = playerInput.actions["Shift"];
        fireAction = playerInput.actions["Fire"];

        moveSpeed = baseMoveSpeed;
        zeroVelocityJumpTime = jumpSpeed / -Physics.gravity.y;
    }

    private void OnEnable()
    {
        jumpInput.started += OnJumpInput;
        shiftInput.started += OnShiftInput;
        shiftInput.canceled += OnShiftInput;
        fireAction.started += OnFireInput;
    }

    private void OnDisable() => DisableInput();
    
    private void DisableInput()
    {
        jumpInput.started -= OnJumpInput;
        shiftInput.started -= OnShiftInput;
        shiftInput.canceled -= OnShiftInput;
        fireAction.started -= OnFireInput;
    }

    private void Start()
    {
        if (!HasStateAuthority)
        {
            Debug.Log("소유권이 없는 컨트롤 비활성화");
            this.enabled = false;
        }
    }

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

    private void OnShiftInput(InputAction.CallbackContext context)
    {
        moveSpeed = baseMoveSpeed;
        if (context.started)
            moveSpeed *= 0.5f;
    }

    private void OnFireInput(InputAction.CallbackContext _)
    {
        attackComponent?.Fire();
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

        // Debug.Log($"isGrounded: {isGrounded} | SimulationTime: {Runner.SimulationTime} | SimulationDeltaTime: {Runner.DeltaTime} | FallBeginTime: {FallBeginTime}");

        // 점프 또는 낙하 시간으로부터 y축 속도를 계산하는 방식을 사용해보았다
        // 예상 특징: 천장에 막혀도 상승 시간은 보장되는 조작감(플랫포머에서 자주 보는)
        // 예상 단점: 공중에서 다른 요소로 인해 속도가 변하는 상황의 처리가 복잡해짐

        if (moveVector != Vector3.zero)
            transform.forward = moveVector;

        if (isGrounded)
        {
            controller.Move(Runner.DeltaTime * moveSpeed * moveVector);
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.4f);
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
                isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.4f);
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

        if (TryGetComponent(out FPSRenderSetter renderSetter))
        {
            renderSetter.SetShadowsOnly();
        }

        if (TryGetComponent(out NetworkedHealthPoint healthPoint))
        {
            healthPoint.OnDead.AddListener(OnPlayerDead);
        }

    }

    /// <summary>
    /// 컨트롤중인 플레이어 사망시 처리
    /// 다른 플레이어에서도 공통 처리 요소는 PlayerDeadDirector에서 처리중
    /// </summary>
    private void OnPlayerDead()
    {

        this.enabled = false;

        if (Camera.main.TryGetComponent(out CameraController camControl))
        {
            camControl.enabled = false;
        }
        Camera.main.transform.SetParent(this.transform);


        TurnOffSyncRpc();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void TurnOffSyncRpc()
    {
        if (TryGetComponent(out NetworkTransform networkTransform))
        {
            networkTransform.enabled = false;
        }

        if (TryGetComponent(out NetworkObject networkObject))
        {
            networkObject.enabled = false;
        }
    }
}

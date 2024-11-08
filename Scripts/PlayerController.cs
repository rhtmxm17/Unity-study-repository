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

    private Vector3 inputVector;

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

        zeroVelocityJumpTime = jumpSpeed / Physics.gravity.y;
    }


    //// ����� �Է��� ó���ϴ� ������Ʈ�� ��Ȯ�� �и��Ѵٸ� �ƿ� ����/��Ȱ��ȭ�ϴ� �͵� ��ȿ
    //private void Start()
    //{
    //    if (!HasStateAuthority)
    //    {
    //        Debug.Log("�������� ���� ��Ʈ�� �ı�");
    //        Destroy(this);
    //    }
    //}

    // �ش� Ŭ���̾�Ʈ������ ����Ǵ� Update �ֱ�
    private void Update()
    {
        //// �������� ���� NetworkBehaviour�� Update�� ȣ���. ����
        //Debug.Log($"Update ������: {HasStateAuthority}");

        if (! HasStateAuthority)
            return;

        Vector2 input = moveInput.ReadValue<Vector2>();
        inputVector.x = input.x;
        inputVector.z = input.y;
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        if (false == isGrounded)
            return;

        isGrounded = false;
        FallBeginTime = Runner.SimulationTime + zeroVelocityJumpTime; // ���� �ӵ��� 0�� �Ǵ� �ð��� ����
    }

    // ��Ʈ��ũ ��� �ֱ⸶�� Network Object�� ���� ȣ���
    // ��Ʈ��ũ ����Ŭ �󿡼� Updateó�� ����
    public override void FixedUpdateNetwork()
    {
        //// �׻� true ��µ�. �������� ���� ��ü�� FixedUpdateNetwork�� ȣ��Ǵµ�?
        //Debug.Log($"FixedUpdateNetwork ������: {HasStateAuthority}");

        // �׷��� �Ŵ��󿡼� ����ó���� �ϰ������� �����°� ���� �� ����
        if (! HasStateAuthority)
            return;

        Debug.Log($"isGrounded: {isGrounded} | SimulationTime: {Runner.SimulationTime} |FallBeginTime: {FallBeginTime}");

        if (isGrounded)
        {
            controller.Move(Runner.DeltaTime * moveSpeed * inputVector);
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
            if (false == isGrounded)
            {
                FallBeginTime = Runner.SimulationTime; // ���� �ӵ��� 0�� �Ǵ� �ð� == ����
            }
        }
        else
        {
            Vector3 velocity = moveSpeed * inputVector;
            velocity.y = Physics.gravity.y * (Runner.SimulationTime - FallBeginTime);
            controller.Move(Runner.DeltaTime * velocity);

            // ���� ���۽� isGrounded�� �Ǹ� �ȵǹǷ� �������� ��쿡�� isGrounded �˻�
            if (velocity.y <= 0f)
            {
                isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
            }
        }
    }

    // ��Ʈ��ũ ������Ʈ ������ ȣ���
    public override void Spawned()
    {
        // �������� ���� NetworkBehaviour�� Spawned�� ȣ���
        Debug.Log($"Spawned ������: {HasStateAuthority}");

        // �� Ŭ���̾�Ʈ�� �ٸ� ����� ĳ���Ͱ� ��Ÿ���� ������ �ؾ� �� ó��

        if (! HasStateAuthority)
            return;

        // �� ĳ���Ϳ� ���ؼ��� �ؾ� �� ó��(��: ī�޶� ���� ��� ����)
        if (Camera.main.TryGetComponent(out CameraController camControl))
        {
            camControl.Target = this.transform;
        }
    }

}

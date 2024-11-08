using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    
    private CharacterController controller;
    private InputAction moveInput;
    private InputAction jumpInput;

    private Vector3 moveVector;

    private float jumpBeginHeight;
    private float jumpBeginTime;

    private bool isGrounded = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        PlayerInput playerInput = PlayerInput.GetPlayerByIndex(0);
        moveInput = playerInput.actions["Move"];
        moveInput = playerInput.actions["Jump"];
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
        moveVector.x = moveSpeed * input.x;
        moveVector.z = moveSpeed * input.y;
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

        if (isGrounded)
        {
            controller.SimpleMove(Runner.DeltaTime * moveVector);
        }
        else
        {

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
    }

}

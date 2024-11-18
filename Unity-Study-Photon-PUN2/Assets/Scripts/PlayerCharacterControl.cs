using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerCharacterControl : MonoBehaviourPun
{
    [SerializeField] float speed;

    private CharacterMovement movement;
    private ProjectileShooter shooter;

    private InputAction moveInput;
    private InputAction fireInput;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        shooter = GetComponent<ProjectileShooter>();

        // 1클라이언트 1플레이어 전제
        PlayerInput input = PlayerInput.GetPlayerByIndex(0);

        moveInput = input.actions["Move"];
        fireInput = input.actions["Fire"];
    }

    private void Start()
    {
        Camera.main.GetComponent<CameraController>().Target = this.transform;
    }

    private void OnEnable()
    {
        if (false == photonView.IsMine)
        {
            this.enabled = false;
            return;
        }
        fireInput.started += WhenFireInput;
    }

    private void OnDisable()
    {
        fireInput.started -= WhenFireInput;
    }

    private void WhenFireInput(InputAction.CallbackContext _)
    {
        shooter.Fire(8f);
    }

    private void Update()
    {
        // 이동 입력 처리

        Vector3 inputAxisX = Camera.main.transform.right; // 좌우 입력에 대응하는 월드 방향
        Vector3 inputAxisY = Camera.main.transform.forward; // 전후 입력에 대응하는 월드 방향

        inputAxisX.y = 0f;
        inputAxisY.y = 0f;

        Vector3 cameraLookXZ = inputAxisY;

        inputAxisX.Normalize();
        inputAxisY.Normalize();

        Vector2 inputVector = moveInput.ReadValue<Vector2>();
        Vector3 moveDirection = inputAxisX * inputVector.x + inputAxisY * inputVector.y;
        movement.Move(Time.deltaTime * speed * moveDirection);

        // 카메라 정면 방향 바라보기
        movement.LookDirection(cameraLookXZ);
    }
}

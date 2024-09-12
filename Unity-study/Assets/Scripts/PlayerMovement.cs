using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("기준 좌표 공간\n대체로 카메라 혹은 자기자신")] private Transform space;
    [SerializeField, Tooltip("몸통")] private Transform body;
    [SerializeField, Min(0f)] private float movementSpeed = 5f;
    [SerializeField, Tooltip("3차원 이동 여부")] private bool moveY = false;
    [SerializeField] private float rotationDegreesPerSecond = 360f;

    private IMovableModel model;
    private PlayerInput playerInput;

    private void Awake()
    {
        if (space == null)
            space = transform;
        model = GetComponent<IMovableModel>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        // 입력
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 방향 결정
        Vector3 dir = space.rotation * new Vector3(x, 0, z); // 기준 Transform의 방향으로 변환
        if (false == moveY)
            dir.y = 0f;
        dir.Normalize();

        // 이동
        transform.Translate(Time.deltaTime * movementSpeed * dir, Space.World);

        // 회전
        if (false == Input.GetMouseButton(1))
        {
            Vector3 spaceForward = space.forward;
            if (false == moveY)
                spaceForward.y = 0f;

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(spaceForward),
                Time.deltaTime * rotationDegreesPerSecond);

        }
        model.MoveSpeed = transform.worldToLocalMatrix * (movementSpeed * dir);
    }
}

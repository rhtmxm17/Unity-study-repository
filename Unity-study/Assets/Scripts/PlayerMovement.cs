using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Tooltip("기준 좌표 공간\n대체로 카메라 혹은 자기자신")] private Transform space;
    [SerializeField, Tooltip("몸통")] private Transform body;
    [SerializeField, Min(0f)] private float movementSpeed = 5f;
    [SerializeField, Tooltip("3차원 이동 여부")] private bool moveY = false;
    [SerializeField] private float rotationDegreesPerSecond = 360f;

    [SerializeField] private Animator animator;

    private void Update()
    {
        // 입력
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 방향 결정
        Vector3 dir = space.rotation * new Vector3(x, 0, z); // 기준 Transform의 방향으로 변환
        if (false == moveY)
            dir.y = 0f;
        if (dir == Vector3.zero)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }
        dir.Normalize();
        animator.SetFloat("Speed", movementSpeed);

        // 이동
        transform.Translate(movementSpeed * Time.deltaTime * dir, Space.World);

        if (body != null)
        {
            // 회전
            body.rotation = Quaternion.RotateTowards
            (
                body.rotation, // 현재 방향
                Quaternion.LookRotation(dir), // 바라볼 방향
                Time.deltaTime * rotationDegreesPerSecond // 최대 회전
            );
        }
    }
}

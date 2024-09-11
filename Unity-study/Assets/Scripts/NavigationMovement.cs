using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class NavigationMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private PlayerInput input;
    private LayerMask clickMask; // 클릭 가능한 대상

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        clickMask = ~LayerMask.GetMask("Player", "Ignore Raycast");
    }

    private void Start()
    {
        // Action이 있다!
        // 자체 기능으로 매 프레임 KeyDown 확인하지 않아도 됨
        input.actions["RightClick"].performed += MoveToClick;
    }

    private void OnDestroy()
    {
        input.actions["RightClick"].performed -= MoveToClick;
    }


    private void Update()
    {
        // 속도 정보를 Animator로 전달
        Vector3 localSpeed = transform.InverseTransformVector(agent.velocity);
        animator.SetFloat("XSpeed", localSpeed.x);
        animator.SetFloat("ZSpeed", localSpeed.z);
    }

    // 클릭시 해당 지점으로 이동
    private void MoveToClick(InputAction.CallbackContext context)
    {
        Vector2 cursor = input.actions["Point"].ReadValue<Vector2>(); // 커서 위치 받아오기
        Debug.Log($"클릭 좌표: {cursor}");
        Ray clickRay = input.camera.ScreenPointToRay(cursor); // 카메라를 통해 Ray로 변환

        if (Physics.Raycast(clickRay, out RaycastHit hitInfo, 50f, clickMask))
        {
            agent.destination = hitInfo.point;
            Debug.Log($"목적지 좌표: {hitInfo.point}");
        }
    }
}

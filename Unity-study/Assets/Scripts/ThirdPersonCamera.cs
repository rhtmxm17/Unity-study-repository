using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Tooltip("카메라 기준으로 타겟을 어디에 둘 것인지 결정")] private Vector3 targetInLocal;
    [SerializeField] private float sensitivityX = 5f;
    [SerializeField] private float sensitivityY = 5f;

    private float dotUpMax;
    private float dotUpMin;

    private void Awake()
    {
        dotUpMax = Mathf.Sin(Mathf.Deg2Rad * 80f);
        dotUpMin = Mathf.Sin(Mathf.Deg2Rad * -80f);
    }

    private void LateUpdate()
    {
        float xInput = Input.GetAxis("Mouse X") * sensitivityX;
        float yInput = Input.GetAxis("Mouse Y") * sensitivityY;

        transform.Rotate(Vector3.left, yInput, Space.Self); // left축으로 시계방향 == 고개 들기
        transform.Rotate(Vector3.up, xInput, Space.World); // up축으로 시계방향 == 오른쪽

        if (transform.forward.y > dotUpMax)
        {
            Debug.Log("목꺾임");
        }
        else if (transform.forward.y < dotUpMin)
        {
            Debug.Log("목접힘");
        }

        Vector3 from = target.position - transform.rotation * targetInLocal;
        transform.position = from;
    }
}

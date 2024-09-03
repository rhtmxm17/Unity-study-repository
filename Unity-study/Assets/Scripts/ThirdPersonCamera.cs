using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Tooltip("카메라 기준으로 타겟을 어디에 둘 것인지 결정")] private Vector3 targetInLocal;
    [SerializeField] private float sensitivityX = 5f;
    [SerializeField] private float sensitivityY = 5f;

    [SerializeField] private float lookupAngleMax = 80f;
    [SerializeField] private float lookupAngleMin = -80f;

    private float dotUpMax;
    private float dotUpMin;

    private float horizontalAngleCur = 0f;
    private float verticalAngleCur = 0f;

    private void Awake()
    {
        dotUpMax = Mathf.Sin(Mathf.Deg2Rad * lookupAngleMax);
        dotUpMin = Mathf.Sin(Mathf.Deg2Rad * lookupAngleMin);
    }

    private void Update()
    {
        float xInput = Input.GetAxis("Mouse X") * sensitivityX;
        float yInput = Input.GetAxis("Mouse Y") * sensitivityY;

        //RotateLockDot(xInput, yInput);
        //RotateLockSetAndRotate(xInput, yInput);
        RotateLockSet(xInput, yInput);
    }

    private void RotateLockDot(float xInput, float yInput)
    {
        // 돌려보고 검사 하는 방법이라 제한에 걸리면 부들부들 떨린다
        // 되돌리는 코드 또한 정확하지 않다. 정확히 하려면
        // 삼각함수 역함수로 각도를 계산하고 보정해서
        // 다시 넣어줘야 할 것 같은데 비용도 걱정된다

        transform.Rotate(Vector3.left, yInput, Space.Self); // left축으로 시계방향 == 고개 들기
        transform.Rotate(Vector3.up, xInput, Space.World); // up축으로 시계방향 == 오른쪽

        // transform.forward.y ==  Vector3.Dot(transform.forward, Vector3.up)
        // 상하방향 회전의 지표로 사용
        if (transform.forward.y > dotUpMax)
        {
            Debug.Log("상한값");
            transform.LookAt(transform.position + new Vector3(transform.forward.x, dotUpMax, transform.forward.z));
        }
        else if (transform.forward.y < dotUpMin)
        {
            Debug.Log("하한값");
            transform.LookAt(transform.position + new Vector3(transform.forward.x, dotUpMin, transform.forward.z));
        }
    }

    private void RotateLockSetAndRotate(float xInput, float yInput)
    {
        // transform의 'Set 함수 호출, rotation에 값 대입'과 'Rotate 함수 호출'은 동시에 사용이 불가능해보임
        // Translate와 Rotate와 같은 함수는 물리 엔진과의 호환이 전제되있기 때문인지
        // 대입 이전의 값을 가져와 되돌아오는 모습을 보인다.

        // 수직방향 각도 갱신 및 제한
        verticalAngleCur += yInput;
        if (verticalAngleCur > lookupAngleMax)
            verticalAngleCur = lookupAngleMax;
        if (verticalAngleCur < lookupAngleMin)
            verticalAngleCur = lookupAngleMin;

        transform.Rotate(Vector3.up, xInput, Space.World); // up축으로 시계방향 == 오른쪽
        transform.SetPositionAndRotation(transform.position, Quaternion.AngleAxis(verticalAngleCur, Vector3.right));
    }

    private void RotateLockSet(float xInput, float yInput)
    {
        // 수직방향 각도 갱신 및 제한
        horizontalAngleCur += xInput;
        verticalAngleCur -= yInput;
        if (verticalAngleCur > lookupAngleMax)
            verticalAngleCur = lookupAngleMax;
        if (verticalAngleCur < lookupAngleMin)
            verticalAngleCur = lookupAngleMin;

        transform.rotation = Quaternion.Euler(verticalAngleCur, horizontalAngleCur, 0f);
    }

    private void LateUpdate()
    {
        Vector3 from = target.position - transform.rotation * targetInLocal;
        transform.position = from;
    }
}

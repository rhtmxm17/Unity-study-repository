using UnityEngine;

public class TankControl : MonoBehaviour
{
    [Header("이동 수치")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("조준 수치")]
    [SerializeField] private float cannonHorizontalSpeed;
    [SerializeField] private float cannonVerticalSpeed;

    private float cannonMaxAngle = 80f;
    private float cannonCurrentAngle = 10f;

    [Header("캐논 초기화")]
    [SerializeField] private Transform cannonBase; // 좌우 회전 조준
    [SerializeField] private Transform cannonJoint; // 상하 회전 조준
    [SerializeField] private Transform muzzlePoint; // 발사 지점

    [SerializeField] private CannonBall cannonBallPrototype;
    
    private ObjectPool<CannonBall> cannonBallPool;
    private Rigidbody rigid;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        cannonBallPool = new(cannonBallPrototype, 5, transform);
    }

    private void Update()
    {
        Movement();
        TakeAim();
        Shoot();
    }

    private void Movement()
    {
        // 좌우 회전
        int rotation = 0; // 시계방향: +1, 반시계: -1
        if (Input.GetKey(KeyCode.D))
            rotation++;
        if (Input.GetKey(KeyCode.A))
            rotation--;

        // 각속도 설정시 속도 상한 존재?
        //rigid.angularVelocity = rotation * rotationSpeed * transform.up;
        transform.Rotate(Vector3.up, rotation * rotationSpeed * Time.deltaTime, Space.Self);

        // 전후 이동
        int moveFoward = 0;
        if (Input.GetKey(KeyCode.W))
            moveFoward++;
        if (Input.GetKey(KeyCode.S))
            moveFoward--;

        rigid.velocity = moveFoward * movementSpeed * transform.forward;

    }

    private void TakeAim()
    {
        // 포신 좌우 회전
        int horizontal = 0;
        if (Input.GetKey(KeyCode.RightArrow)) // y축 회전 기준 시계
            horizontal++;
        if (Input.GetKey(KeyCode.LeftArrow)) // y축 회전 기준 반시계
            horizontal--;

        cannonBase.Rotate(Vector3.up, horizontal * cannonHorizontalSpeed * Time.deltaTime, Space.Self);

        // 포신 상하 회전
        int vertical = 0;
        if (Input.GetKey(KeyCode.UpArrow))
            vertical++;
        if (Input.GetKey(KeyCode.DownArrow))
            vertical--;

        // 현재 각도를 가져와서 제한하기 어렵다 
        //cannonJoint.Rotate(Vector3.left, vertical * cannonVerticalSpeed * Time.deltaTime, Space.Self);

        cannonCurrentAngle += vertical * cannonVerticalSpeed * Time.deltaTime;
        if (cannonCurrentAngle < 0f)
            cannonCurrentAngle = 0f;
        if (cannonCurrentAngle > cannonMaxAngle)
            cannonCurrentAngle = cannonMaxAngle;

        // 위쪽 방향이 +가 되도록(시계방향) 하기 위해 축으로 left 사용
        cannonJoint.localRotation = Quaternion.AngleAxis(cannonCurrentAngle, Vector3.left);

    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CannonBall ball = cannonBallPool.PopPool(3f);
            if (ball == null)
                return;

            ball.transform.SetPositionAndRotation(muzzlePoint.position, muzzlePoint.rotation);
            ball.Rigid.AddForce(ball.transform.forward * 5f, ForceMode.Impulse);
        }
    }
}

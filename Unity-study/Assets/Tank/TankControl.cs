using UnityEngine;
using UnityEngine.Events;

public class TankControl : MonoBehaviour
{
    [Header("이동 수치")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("조준 수치")]
    [SerializeField] private float cannonHorizontalSpeed;
    [SerializeField] private float cannonVerticalSpeed;

    private float cannonMaxAngle = 60f;
    private float cannonCurrentAngle = 10f;

    [Header("캐논 초기화")]
    [SerializeField] private Transform cannonBase; // 좌우 회전 조준
    [SerializeField] private Transform cannonJoint; // 상하 회전 조준
    [SerializeField] private Transform muzzlePoint; // 발사 지점

    [SerializeField] private CannonBall[] cannonBallPrototypes;
    [field:SerializeField] public int Selected { get; private set; }
    
    private ObjectPool<CannonBall>[] cannonBallPool;
    private Rigidbody rigid;

    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        cannonBallPool = new ObjectPool<CannonBall>[cannonBallPrototypes.Length];
        for (int i = 0; i < cannonBallPrototypes.Length; i++)
        {
            cannonBallPool[i] = new(cannonBallPrototypes[i], 5, transform);
        }
    }

    private void Update()
    {
        Movement();
        TakeAim();
        SelectCannonBall();
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

        //// case1: velocity 갱신
        //// 이동 속도를 강제함, 경사 진입 불가능 및 y값 초기화로 인해 낙하 거의 없음(플라잉 탱크)
        //rigid.velocity = moveFoward * movementSpeed * transform.forward;

        //// case2: velocity zx평면 갱신
        //// 경사에서 잘 넘어짐
        //Vector3 move = moveFoward * movementSpeed * transform.forward;
        //rigid.velocity = new Vector3(move.x, rigid.velocity.y, move.z);

        // case3: translate 사용
        transform.Translate(moveFoward * movementSpeed * Time.deltaTime * Vector3.forward, Space.Self);
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

    private void SelectCannonBall()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Selected = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Selected = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Selected = 2;
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CannonBall ball = cannonBallPool[Selected].PopPool(3f);
            if (ball == null)
                return;

            ball.transform.SetPositionAndRotation(muzzlePoint.position, muzzlePoint.rotation);
            ball.Rigid.AddForce(ball.transform.forward * 10f, ForceMode.Impulse);
        }
    }
}

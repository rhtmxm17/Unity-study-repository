using UnityEngine;

public class FPS_PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float movementSpeed = 5f;


    private LayerMask hitableMask;

    private void Awake()
    {
        hitableMask = MyUtil.maskMonster | MyUtil.maskDefault;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Movement();
        TakeAim();
        Shoot();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0, z).normalized;
        transform.Translate(movementSpeed * Time.deltaTime * dir, Space.Self);
    }

    private void TakeAim()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        head.Rotate(Vector3.left, y, Space.Self); // left축으로 시계방향 == 고개 들기
        transform.Rotate(Vector3.up, x, Space.Self); // up축으로 시계방향 == 오른쪽
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("발사");

            if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit info, 100f, hitableMask))
            {
                // rigidbody가 있다면 충돌지점이 전체 게임오브젝트의 일부인 것으로 보고 rigidbody의 gameObject를 사용
                GameObject hitted = (info.rigidbody?.gameObject) ?? info.collider.gameObject;

                if (hitted.layer == MyUtil.layerDefault)
                {
                    Debug.Log("지형 적중");
                }
                else if (hitted.layer == MyUtil.layerMonster)
                {
                    Debug.Log("표적 적중");
                    if (hitted.TryGetComponent(out Target target))
                    {
                        target.Damaged(1);
                    }
                }

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, 0.5f);
        Gizmos.DrawWireCube(transform.TransformPoint(0f, 0.5f, 0f), Vector3.one);

        Gizmos.DrawRay(muzzle.position, muzzle.forward);
        if (Input.GetButton("Fire1"))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 100f);
        }
    }
}

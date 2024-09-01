using System.Collections;
using UnityEngine;

public class FPS_PlayerControl : MonoBehaviour
{

    [SerializeField] private Transform head;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float movementSpeed = 5f;

    [Header("사격 설정")]
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private float secondsPerFire = 0.1f;
    [SerializeField] private float reloadTime = 2f;

    private enum FireState { Idle, Shooting, Reloading, };
    private FireState fireState;

    private LayerMask hitableMask;
    private WaitForSeconds waitFire;
    private WaitForSeconds waitReload;
    private Coroutine fireRoutine;
    private Coroutine reloadRoutine;

    private int loaded;

    private void Awake()
    {
        fireState = FireState.Idle;
        hitableMask = MyUtil.maskMonster | MyUtil.maskDefault;
        waitFire = new(secondsPerFire);
        waitReload = new(reloadTime);
        loaded = magazineSize;
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
        FireCheck();
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

    private void FireCheck()
    {
        switch (fireState)
        {
            case FireState.Idle:
                if (Input.GetButtonDown("Fire1"))
                {
                    fireRoutine = StartCoroutine(StartFire());
                }
                else if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Fire2"))
                {
                    reloadRoutine = StartCoroutine(Reloading());
                }
                break;
            case FireState.Shooting:
                if (Input.GetButtonUp("Fire1"))
                {
                    fireState = FireState.Idle;
                    StopCoroutine(fireRoutine);
                    fireRoutine = null;
                }
                break;
            case FireState.Reloading:
                if (Input.GetButtonDown("Fire1"))
                    Debug.Log("장전 진행중!");
                break;
            default:
                Debug.LogError("정의되지 않은 발사 상태");
                break;
        }

    }

    private IEnumerator StartFire()
    {
        fireState = FireState.Shooting;
        while (loaded > 0)
        {
            Fire();
            yield return waitFire;
        }
        fireState = FireState.Idle;
        fireRoutine = null;
        EmptyMagazine();
    }

    private void EmptyMagazine() => Debug.Log("탄창이 소진됨!");

    private IEnumerator Reloading()
    {
        fireState = FireState.Reloading;
        yield return waitReload;
        loaded = magazineSize;
        fireState = FireState.Idle;
    }

    private void Fire()
    {
        loaded--;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, 0.5f);
        Gizmos.DrawWireCube(transform.TransformPoint(0f, 0.5f, 0f), Vector3.one);

        Gizmos.DrawRay(muzzle.position, muzzle.forward);
        if (fireState == FireState.Shooting)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 100f);
        }
    }
}

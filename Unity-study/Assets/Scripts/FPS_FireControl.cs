using System.Collections;
using UnityEngine;

public class FPS_FireControl : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private ParticleSystem muzzleParticle;
    [SerializeField] private ParticleSystem bulletImpactPrefab;

    [Header("사격 설정")]
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private float secondsPerFire = 0.1f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private FireFlag fireLock;

    private ShooterPlayerModel model;

    [System.Flags]
    public enum FireFlag
    {
        Reloading = 1 << 0,
        ExtraLock = 1 << 2,
        ZoomLock = 1 << 3,

    }

    public int Loaded
    {
        get => model.Bullets;
        private set => model.Bullets = value;
    }

    public bool ZoomLock
    {
        get => 0 != (fireLock & FireFlag.ZoomLock);
        set
        {
            if (value)
            {
                fireLock |= FireFlag.ZoomLock;
                if (fireRoutine != null)
                {
                    StopCoroutine(fireRoutine);
                    fireRoutine = null;
                }
            }
            else
            {
                fireLock &= ~FireFlag.ZoomLock;
            }
        }
    }

    private LayerMask hitableMask;
    private WaitForSeconds waitFire;
    private WaitForSeconds waitReload;
    private Coroutine fireRoutine;
    private Coroutine reloadRoutine;

    private void Awake()
    {
        model = GetComponent<ShooterPlayerModel>();
        hitableMask = MyUtil.maskMonster | MyUtil.maskDefault;
        waitFire = new(secondsPerFire);
        waitReload = new(reloadTime);
    }

    private void Start()
    {
        Loaded = magazineSize;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        FireCheck();
    }

    private void FireCheck()
    {
        if (0 == fireLock)
        {
            if (Input.GetButtonDown("Fire1") && fireRoutine == null)
            {
                fireRoutine = StartCoroutine(StartFire());
            }
            if (Input.GetButtonUp("Fire1") && fireRoutine != null)
            {
                StopCoroutine(fireRoutine);
                fireRoutine = null;
            }
        }


        if (0 == (fireLock & FireFlag.Reloading))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                reloadRoutine = StartCoroutine(Reloading());
            }
        }
    }

    private IEnumerator StartFire()
    {
        while (Loaded > 0)
        {
            Fire();
            yield return waitFire;
        }
        fireRoutine = null;
        EmptyMagazine();
    }

    private void EmptyMagazine() => Debug.Log("탄창이 소진됨!");

    private IEnumerator Reloading()
    {
        fireLock |= FireFlag.Reloading;
        yield return waitReload;
        Loaded = magazineSize;
        fireLock &= ~FireFlag.Reloading;
    }

    private void Fire()
    {
        Loaded--;
        model.TriggerFire();

        muzzleParticle.transform.SetPositionAndRotation(muzzle.position, muzzle.rotation * Quaternion.FromToRotation(Vector3.right, Vector3.forward));
        muzzleParticle.Play();

        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit info, 100f, hitableMask))
        {
            // rigidbody가 있다면 충돌지점이 전체 게임오브젝트의 일부인 것으로 보고 rigidbody의 gameObject를 사용
            GameObject hitted = (info.rigidbody != null) ? info.rigidbody.gameObject : info.collider.gameObject;

            if (hitted.layer == MyUtil.layerDefault)
            {
                Debug.Log("지형 적중");
                var particle = Instantiate(bulletImpactPrefab);
                Destroy(particle.gameObject, 5f);

                particle.transform.SetPositionAndRotation
                (
                    info.point, // 적중 지점
                    Quaternion.LookRotation(Vector3.Reflect(muzzle.forward, info.normal)) // 반사각에 맞춰서 회전
                );

                // 표면에 맞춰서 회전
                particle.transform.GetChild(1).rotation = Quaternion.LookRotation(-info.normal);
            }
            else if (hitted.layer == MyUtil.layerMonster)
            {
                Debug.Log("표적 적중");
                if (hitted.TryGetComponent(out IDamagedable target))
                {
                    target.Damaged(1);
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.TransformPoint(0f, 0.5f, 0f), Vector3.one);

        Gizmos.DrawRay(muzzle.position, muzzle.forward);
        if (fireRoutine != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(muzzle.position, muzzle.position + muzzle.forward * 100f);
        }
    }
}

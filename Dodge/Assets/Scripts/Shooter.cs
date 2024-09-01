using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("총알 정보")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float shootPeriod = 1f;
    private float remainShootTime;
    [SerializeField] private Transform muzzle;

    private Transform target;

    void Start()
    {
        // 게임 상태 구독
        GameManager.Instance.OnGameStateChanged.AddListener(WhenGameStateChanged);
        WhenGameStateChanged(GameManager.Instance.State); // 최초 1회 현재 상태에 따른 설정 필요

        // 발사 정보 초기화
        remainShootTime = shootPeriod;
        if (muzzle == null)
            muzzle = transform; //인스펙터에서 총구 위치가 설정되지 않았을 경우 자신의 위치

        // 타겟 설정
        target = GameObject.FindWithTag("Player")?.transform;
        if (target == null)
            Debug.LogError("Player가 존재하지 않음");
    }

    void Update()
    {
        remainShootTime -= Time.deltaTime;

        if (remainShootTime <= 0)
        {
            Shoot();

            remainShootTime += shootPeriod;
        }
    }

    void WhenGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Ready:
            case GameManager.GameState.GameOver:
                this.enabled = false;
                break;
            case GameManager.GameState.Running:
                this.enabled = true;
                break;
            default:
                break;
        }
    }

    void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Shoot(target);
    }
}

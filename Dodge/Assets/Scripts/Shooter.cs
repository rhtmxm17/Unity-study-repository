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
        remainShootTime = shootPeriod;
        if (muzzle == null)
            muzzle = transform; //인스펙터에서 총구 위치가 설정되지 않았을 경우 자신의 위치

        target = GameObject.FindWithTag("Player")?.transform;
        if (target == null)
            Debug.LogError("Player가 존재하지 않음");
    }

    // Update is called once per frame
    void Update()
    {
        remainShootTime -= Time.deltaTime;

        if (remainShootTime <= 0)
        {
            Shoot();

            remainShootTime += shootPeriod;
        }
    }

    void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Shoot(target);
    }
}

using System.Collections;
using UnityEngine;

namespace Test06
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;

        [Header("이동 수치")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationDegreesPerSecond = 180f;

        [Space]
        [SerializeField] private float fireCycleTime = 0.5f;

        private float lastFireTime;
        private Coroutine fireRoutine;
        private WaitForSeconds waitFire;
        private WaitForSeconds waitDestroyBullet;

        private Rigidbody rigid;

        private void Awake()
        {
            waitFire = new(fireCycleTime);
            waitDestroyBullet = new(2f);

            if (false == TryGetComponent(out rigid))
            {
                Debug.LogError("RigidBody가 필요함");
            }
        }

        private void Update()
        {
            Movement();
            InputFire();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
            {
                rigid.constraints = RigidbodyConstraints.None;
                this.enabled = false;

                GameManager.Instance.State = GameManager.GameState.GameOver;
            }
        }

        private void Movement()
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            if (moveDir == Vector3.zero)
                return;

            transform.Translate(Time.deltaTime * movementSpeed * moveDir, Space.World);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, // 현재 방향
                Quaternion.LookRotation(moveDir), // 바라볼 방향
                Time.deltaTime * rotationDegreesPerSecond // 최대 회전
                );
        }

        private void InputFire()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                fireRoutine = StartCoroutine(FireRoutine());
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                StopCoroutine(fireRoutine);
            }
        }

        private IEnumerator FireRoutine()
        {
            // 연타 대처
            if (Time.time - lastFireTime < fireCycleTime)
                yield return new WaitForSeconds(Time.time - lastFireTime);

            while (true)
            {
                Fire();
                lastFireTime = Time.time;
                yield return waitFire;
            }
        }

        private void Fire()
        {
            Bullet bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(5f * bullet.transform.forward, ForceMode.Impulse);
            StartCoroutine(DestroyBullet(bullet));
        }

        private IEnumerator DestroyBullet(Bullet bullet)
        {
            GameObject target = bullet.gameObject;
            yield return waitDestroyBullet;

            // 왜 에러가 안나지?
            // 몬스터에 적중했다면 이미 삭제된 오브젝트를 파괴하는데 오류가 안생김
            Destroy(target);
        }
    }
}

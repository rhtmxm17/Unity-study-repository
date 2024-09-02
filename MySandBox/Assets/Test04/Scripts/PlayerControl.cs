using System.Collections;
using UnityEngine;

namespace Test04
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;

        [Header("이동 수치")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationDegreesPerSecond = 180f;

        private Coroutine fireRoutine;
        private WaitForSeconds waitFire;
        private WaitForSeconds waitDestroyBullet;
        //private Rigidbody rigid;

        private void Awake()
        {
            waitFire = new(0.5f);
            waitDestroyBullet = new(2f);

            //if (false == TryGetComponent(out rigid))
            //{
            //    Debug.LogError("RigidBody가 필요함");
            //}
        }


        private void Update()
        {
            Movement();
            InputFire();
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
            while (true)
            {
                Fire();
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

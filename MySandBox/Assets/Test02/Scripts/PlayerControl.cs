using System.Collections;
using UnityEngine;

namespace Test02
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform muzzle;

        [Header("이동 수치")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationDegreesPerSecond = 180f;

        private WaitForSeconds waitDestroyBullet;
        //private Rigidbody rigid;

        private void Awake()
        {
            waitDestroyBullet = new(2f);

            //if (false == TryGetComponent(out rigid))
            //{
            //    Debug.LogError("RigidBody가 필요함");
            //}
        }


        private void Update()
        {
            Movement();
            Fire();
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

        private void Fire()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
                bullet.GetComponent<Rigidbody>().velocity = 5f * bullet.transform.forward;
                StartCoroutine(DestroyBullet(bullet));
            }
        }

        private IEnumerator DestroyBullet(GameObject bullet)
        {
            yield return waitDestroyBullet;

            Destroy(bullet);
        }
    }
}

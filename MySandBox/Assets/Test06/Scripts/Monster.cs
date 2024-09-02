using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test06
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private Transform eye;

        [Header("능력치")]
        [SerializeField] private float sightRange = 5f;
        [SerializeField] private float movementSpeed = 1f;
        private float rotationDegreesPerSecond = 360f;

        private Transform target;
        private LayerMask ignoreRaycast;

        private Vector3 toTargetVector;

        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            ignoreRaycast = ~LayerMask.GetMask("Bullets", "Monsters");
        }

        private void Update()
        {
            toTargetVector = target.position - eye.position;
            toTargetVector.y = 0;
            if (toTargetVector == Vector3.zero)
                return;

            toTargetVector.Normalize();
            if (Physics.Raycast(eye.position, toTargetVector, out RaycastHit info, sightRange, ignoreRaycast) // Raycast 적중
                && null != info.rigidbody // null 체크
                && info.rigidbody.CompareTag("Player")) // 플레이어 확인
            {
                // 추적
                transform.Translate(Time.deltaTime * movementSpeed * toTargetVector, Space.World);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, // 현재 방향
                    Quaternion.LookRotation(toTargetVector), // 바라볼 방향
                    Time.deltaTime * rotationDegreesPerSecond // 최대 회전
                    );
            }
            else
            {
                // 직진
                transform.Translate(Time.deltaTime * movementSpeed * Vector3.forward, Space.Self);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                Destroy(this.gameObject);
            }
            else
            {
                // 총알 이외에 충돌시 무작위 회전
                transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(eye.position, toTargetVector * sightRange);
        }
    }
}
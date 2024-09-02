using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test05
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] Transform eye;
        private float movementSpeed = 1f;
        private float rotationDegreesPerSecond = 360f;

        private Transform target;
        private LayerMask ignoreRaycast;

        private Vector3 toTarget;

        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            ignoreRaycast = ~LayerMask.GetMask("Bullets", "Monsters");
        }

        private void Update()
        {
            toTarget = target.position - eye.position;
            toTarget.y = 0;
            if (toTarget == Vector3.zero)
                return;

            toTarget.Normalize();
            if (Physics.Raycast(eye.position, toTarget, out RaycastHit info, 5f, ignoreRaycast))
            {
                // Player 또는 그 구성 오브젝트가 아닐경우 return;
                if (null == info.rigidbody || false == info.rigidbody.CompareTag("Player"))
                    return;

                transform.Translate(Time.deltaTime * movementSpeed * toTarget, Space.World);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, // 현재 방향
                    Quaternion.LookRotation(toTarget), // 바라볼 방향
                    Time.deltaTime * rotationDegreesPerSecond // 최대 회전
                    );
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                Destroy(this.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(eye.position, toTarget * 5f);
        }
    }
}
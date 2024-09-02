using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float rotationDegreesPerSecond = 180f;

    //private Rigidbody rigid;

    //private void Awake()
    //{
    //    if (false == TryGetComponent(out rigid))
    //    {
    //        Debug.LogError("RigidBody가 필요함");
    //    }
    //}

    private void Update()
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
}

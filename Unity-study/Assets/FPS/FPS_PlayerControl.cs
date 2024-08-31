using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float movementSpeed = 5f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Movement();
        TakeAim();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, 0, z).normalized;
        transform.Translate(dir * movementSpeed * Time.deltaTime, Space.Self);
    }

    private void TakeAim()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        head.Rotate(Vector3.left, y, Space.Self); // left축으로 시계방향 == 고개 들기
        transform.Rotate(Vector3.up, x, Space.Self); // up축으로 시계방향 == 오른쪽
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, 0.5f);
        Gizmos.DrawWireCube(transform.TransformPoint(0f, 0.5f, 0f), Vector3.one);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform head;

    private void Update()
    {
        Movement();
        TakeAim();
    }

    private void Movement()
    {

    }

    private void TakeAim()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(head.position, 0.5f);
        Gizmos.DrawWireCube(transform.TransformPoint(0f, 0.5f, 0f), Vector3.one);
    }
}

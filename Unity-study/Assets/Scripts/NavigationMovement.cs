using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NavigationMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.destination = new Vector3(4, 0, 4); // 작동 테스트용 목적지
    }

    private void Update()
    {
        Vector3 localSpeed = transform.InverseTransformVector(agent.velocity);
        animator.SetFloat("XSpeed", localSpeed.x);
        animator.SetFloat("ZSpeed", localSpeed.z);
    }
}

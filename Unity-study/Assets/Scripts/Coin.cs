using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator animator;
    private Collider bodyCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bodyCollider.enabled = false;
            animator.SetTrigger("GetCoin");
            Debug.Log("코인 획득");
            Destroy(gameObject, 2f);
        }
    }
}

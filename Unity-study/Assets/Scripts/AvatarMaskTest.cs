using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMaskTest : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            animator.SetBool("Punch", true);

        if (Input.GetMouseButtonUp(0))
            animator.SetBool("Punch", false);
    }
}

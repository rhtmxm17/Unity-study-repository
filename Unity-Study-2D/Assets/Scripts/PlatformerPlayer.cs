using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlatformerPlayer : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody2D body;
    private SpriteRenderer sprite;

    private Coroutine moveRoutine;
    private YieldInstruction waitPhysics;

    [SerializeField] float speedAccel = 25f;
    [SerializeField] float maxSpeed = 5f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        waitPhysics = new WaitForFixedUpdate();
    }

    private void Start()
    {
        InputAction moveAction = playerInput.actions["MoveX"];
        moveAction.started += context =>
        {
            Debug.Log("moveAction.started");
            moveRoutine = StartCoroutine(MoveRoutine(moveAction));
        };

        moveAction.canceled += context =>
        {
            Debug.Log("moveAction.canceled");
            StopCoroutine(moveRoutine);
        };
    }

    private void Update()
    {
        if (body.velocity.x > 0.01f)
            sprite.flipX = false;
        else if (body.velocity.x < -0.01f)
            sprite.flipX = true;
    }

    private IEnumerator MoveRoutine(InputAction action)
    {
        while(true)
        {
            yield return waitPhysics;
            float value = action.ReadValue<Single>();

            // 가속도
            body.AddForce(value * speedAccel * Vector2.right, ForceMode2D.Force);

            // 한계속도
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxSpeed, maxSpeed), body.velocity.y);
        }
    }
}

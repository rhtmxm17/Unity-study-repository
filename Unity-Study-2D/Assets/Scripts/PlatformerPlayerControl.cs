using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(PlatformerPlayerModel))]
public class PlatformerPlayerControl : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody2D body;
    private PlatformerPlayerModel model;

    private Coroutine moveRoutine;
    private YieldInstruction waitPhysics;

    [SerializeField] float speedAccel = 25f;
    [SerializeField, Tooltip("매 FixedUpdate마다 감쇄량")] float speedDecelLerp = 0.1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float jumpPower = 25f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        model = GetComponent<PlatformerPlayerModel>();

        waitPhysics = new WaitForFixedUpdate();
    }

    private void Start()
    {
        moveRoutine = StartCoroutine(DecelRoutine());

        InputAction moveAction = playerInput.actions["MoveX"];
        moveAction.started += context =>
        {
            StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(AccelRoutine(moveAction));
        };

        moveAction.canceled += context =>
        {
            StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(DecelRoutine());
        };

        playerInput.actions["Jump"].started += context =>
        {
            body.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
            model.IsGrounded = false;
        };
    }

    private void Update()
    {
        model.Velocity = body.velocity;
    }

    private IEnumerator AccelRoutine(InputAction action)
    {
        while (true)
        {
            yield return waitPhysics;
            float value = action.ReadValue<float>();

            // 가속도
            body.AddForce(value * speedAccel * Vector2.right, ForceMode2D.Force);

            // 한계속도
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxSpeed, maxSpeed), body.velocity.y);
        }
    }

    private IEnumerator DecelRoutine()
    {
        while (true)
        //while (body.velocity.x > 0.1f)
        {
            yield return waitPhysics;

            // 감속 속도
            float decel = Mathf.LerpUnclamped(0f, body.velocity.x, -speedDecelLerp);
            body.AddForce(decel * Vector2.right, ForceMode2D.Force);

        }
    }

    //// 벽과 바닥이 동일한 오브젝트일 경우 오작동
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        Debug.Log(collision.GetContact(0).normal);

    //        if (collision.GetContact(0).normal.y < 0.7) // 약 45도
    //        {
    //            Debug.Log("벽/천장 충돌");
    //            return;
    //        }
            
    //        model.IsGrounded = true;
    //    }
    //}
}

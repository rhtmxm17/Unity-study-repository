using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D), typeof(PlatformerPlayerModel))]
public class PlatformerPlayerControl : MonoBehaviour
{
    [SerializeField] float speedAccel = 25f;
    [SerializeField, Tooltip("매 FixedUpdate마다 감쇄량")] float speedDecelLerp = 0.1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float jumpPower = 25f;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Rigidbody2D body;
    private PlatformerPlayerModel model;

    private Coroutine moveRoutine;
    private YieldInstruction waitPhysics;

    private LayerMask groundLayerMask;

    private enum State { Ground, Jump, Dead, COUNT }
    private State curState;
    private StateBase[] states;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody2D>();
        model = GetComponent<PlatformerPlayerModel>();

        states = new StateBase[(int)State.COUNT];
        states[(int)State.Ground] = new GroundState(this);
        states[(int)State.Jump] = new JumpState(this);

        waitPhysics = new WaitForFixedUpdate();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        moveRoutine = StartCoroutine(DecelRoutine());
        moveAction = playerInput.actions["MoveX"];
        jumpAction = playerInput.actions["Jump"];

        curState = State.Ground;
        states[(int)curState].Enter();
    }

    private void OnDestroy()
    {
        states[(int)curState].Exit();
    }

    private void Update()
    {
        model.Velocity = body.velocity;
    }

    private void ChangeState(State nextState)
    {
        states[(int)curState].Exit();
        curState = nextState;
        states[(int)curState].Enter();
    }

    private bool CheckIsGrounded()
    {
        var result = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayerMask);
        return (null != result.collider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector2.down * 0.5f + (Vector2)transform.position);
    }

    private void Accel(InputAction.CallbackContext context)
    {
        StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(AccelRoutine(context));
    }

    private void Decel(InputAction.CallbackContext context)
    {
        StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(DecelRoutine());
    }

    private IEnumerator AccelRoutine(InputAction.CallbackContext action)
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

    private class GroundState : StateBase
    {
        private PlatformerPlayerControl self;
        private Coroutine groundCheckRoutine;

        public GroundState(PlatformerPlayerControl self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.model.IsGrounded = true;
            groundCheckRoutine = self.StartCoroutine(GroundCheck());
            self.moveAction.started += self.Accel;
            self.moveAction.canceled += self.Decel;
            self.jumpAction.started += Jump;
        }

        public override void Exit()
        {
            self.StopCoroutine(groundCheckRoutine);
            self.moveAction.started -= self.Accel;
            self.moveAction.canceled -= self.Decel;
            self.jumpAction.started -= Jump;
        }

        private IEnumerator GroundCheck()
        {
            while(true)
            {
                yield return null;
                if (false == self.CheckIsGrounded())
                    self.ChangeState(State.Jump);
            }
        }

        private void Jump(InputAction.CallbackContext context)
        {
            self.body.AddForce(self.jumpPower * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private class JumpState : StateBase
    {
        private PlatformerPlayerControl self;
        private Coroutine groundCheckRoutine;

        public JumpState(PlatformerPlayerControl self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.model.IsGrounded = false;
            groundCheckRoutine = self.StartCoroutine(GroundCheck());
            self.moveAction.started += self.Accel;
            self.moveAction.canceled += self.Decel;
        }

        public override void Exit()
        {
            self.StopCoroutine(groundCheckRoutine);
            self.moveAction.started -= self.Accel;
            self.moveAction.canceled -= self.Decel;
        }

        private IEnumerator GroundCheck()
        {
            while (true)
            {
                yield return null;

                // 떨어지는 도중에만 지면을 확인
                if (self.body.velocity.y <= 0f && self.CheckIsGrounded())
                    self.ChangeState(State.Ground);
            }
        }
    }
}

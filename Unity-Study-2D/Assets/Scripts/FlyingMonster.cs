using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class FlyingMonster : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float detectRange;
    [SerializeField] float attackRange;
    [SerializeField] float weakAngle;

    private Animator animator;
    private Rigidbody2D body;
    private new Collider2D collider;
    private SpriteRenderer sprite;

    private int animatorIndex_Idle;
    private int animatorIndex_Move;
    private LayerMask playerLayerMask;
    private float sinWeakAngle;

    private enum State { Idle, Chase, Attack, Dead, COUNT }
    private State curState;
    private StateBase[] states;

    private Transform target;
    private Vector2 attackDirction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        animatorIndex_Idle = Animator.StringToHash("Idle");
        animatorIndex_Move = Animator.StringToHash("Move");
        playerLayerMask = LayerMask.GetMask("Player");
        sinWeakAngle = Mathf.Sin(weakAngle);

        states = new StateBase[(int)State.COUNT];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Chase] = new ChaseState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Dead] = new DeadState(this);

    }

    private void Start()
    {
        curState = State.Idle;
        states[(int)curState].Enter();
    }

    private void OnDestroy()
    {
        states[(int)curState].Exit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 레이어를 분리해둔 히트박스 트리거시
        if(collision.TryGetComponent(out PlatformerPlayerControl player))
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)this.transform.position).normalized;
            
            // 기준 각도 안쪽일 경우(플레이어가 위에서 충돌)
            if (direction.y > sinWeakAngle)
            {
                this.ChangeState(State.Dead);
            }
            // 다른 각도일 경우
            else
            {
                player.Damaged(1);
            }
        }
    }

    private void ChangeState(State nextState)
    {
        Debug.Log($"{curState}=>{nextState}");
        states[(int)curState].Exit();
        curState = nextState;
        states[(int)curState].Enter();
    }

    private class IdleState : StateBase
    {
        private readonly FlyingMonster self;
        private YieldInstruction detectPeriod = new WaitForSeconds(0.5f);
        private Coroutine detectRoutine;

        public IdleState(FlyingMonster self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.body.velocity = Vector3.zero;
            self.animator.SetTrigger(self.animatorIndex_Idle);
            detectRoutine = self.StartCoroutine(DetectPlayerRoutine());
        }

        public override void Exit()
        {
            self.StopCoroutine(detectRoutine);
        }

        private IEnumerator DetectPlayerRoutine()
        {
            yield return null;
            while (true)
            {
                Collider2D result = Physics2D.OverlapCircle(self.transform.position, self.detectRange, self.playerLayerMask);

                if (result != null)
                {
                    self.target = result.transform;
                    self.ChangeState(State.Chase);
                }

                yield return detectPeriod;
            }
        }
    }

    private class ChaseState : StateBase
    {
        private readonly FlyingMonster self;
        private YieldInstruction detectPeriod = new WaitForSeconds(0.1f);
        private Coroutine chaseRoutine;

        public ChaseState(FlyingMonster self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.animator.SetTrigger(self.animatorIndex_Move);
            chaseRoutine = self.StartCoroutine(ChasePlayerRoutine());
        }

        public override void Exit()
        {
            self.StopCoroutine(chaseRoutine);
        }

        private IEnumerator ChasePlayerRoutine()
        {
            yield return null;
            while (true)
            {
                Collider2D result = Physics2D.OverlapCircle(self.transform.position, self.attackRange, self.playerLayerMask);

                // 공격 범위 내부라면 공격으로 전환
                if (result != null)
                {
                    self.attackDirction = ((Vector2)self.target.position - (Vector2)self.transform.position).normalized;
                    self.ChangeState(State.Attack);
                }
                // 아닐경우 추적 방향 갱신
                else
                {
                    self.body.velocity = ((Vector2)self.target.position + Vector2.up * 2f - (Vector2)self.transform.position).normalized * self.moveSpeed;
                    self.sprite.flipX = (self.body.velocity.x < 0);
                }

                yield return detectPeriod;
            }
        }
    }

    private class AttackState : StateBase
    {
        private readonly FlyingMonster self;
        private YieldInstruction chargeTimer = new WaitForSeconds(1f);
        private YieldInstruction attackTimer = new WaitForSeconds(0.8f);
        private Coroutine attackRoutine;

        public AttackState(FlyingMonster self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.animator.SetTrigger(self.animatorIndex_Move);
            attackRoutine = self.StartCoroutine(AttackMovement());
        }

        public override void Exit()
        {
            self.StopCoroutine(attackRoutine);
        }

        private IEnumerator AttackMovement()
        {
            self.body.velocity = self.attackDirction * -0.5f;
            yield return chargeTimer;
            self.body.velocity = self.attackDirction * 10f;
            yield return attackTimer;
            self.ChangeState(State.Idle);
        }
    }

    private class DeadState : StateBase
    {
        private readonly FlyingMonster self;

        public DeadState(FlyingMonster self)
        {
            this.self = self;
        }

        public override void Enter()
        {
            self.collider.enabled = false;
            self.sprite.flipY = true;
            self.body.velocity = Vector2.down * 0.5f;
            self.animator.speed = 0;

            Destroy(self.gameObject, 1.5f);
        }

        public override void Exit()
        {
        }
    }
}

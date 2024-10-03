using UnityEngine;
using UnityEngine.Events;

public class PlatformerPlayerModel : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Vector2 velocity;
    [SerializeField] bool isGrounded;
    [SerializeField] int hp;

    [Header("Viewer")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;

    private int animatorIndex_Grounded;
    private int animatorIndex_Jump;
    private int animatorIndex_VelocityX;
    private int animatorIndex_VelocityY;
    private int animatorIndex_Die;

    public event UnityAction OnVelocityChanged;
    public event UnityAction OnGroundChanged;
    public event UnityAction OnHpChanged;
    public event UnityAction OnDie;

    public Vector2 Velocity
    {
        get => velocity;
        set { velocity = value; OnVelocityChanged?.Invoke(); }
    }

    public bool IsGrounded
    {
        get => isGrounded;
        set { if (isGrounded == value) return; isGrounded = value; OnGroundChanged?.Invoke(); }
    }

    public int Hp
    {
        get => hp;
        set { hp = value; OnHpChanged?.Invoke(); }
    }

    public void TriggerOnDie()
    {
        OnDie?.Invoke();
    }

    private void Awake()
    {
        if (TryGetComponent(out sprite))
        {
            OnVelocityChanged += FlipSprite;
        }
        if (TryGetComponent(out animator))
        {
            OnGroundChanged += AnimatorSetGrounded;
            OnVelocityChanged += AnimatorSetVelocity;
            OnDie += AnimatorSetDie;

            animatorIndex_Grounded = Animator.StringToHash("Grounded");
            animatorIndex_Jump = Animator.StringToHash("Jump");
            animatorIndex_VelocityX = Animator.StringToHash("VelocityX");
            animatorIndex_VelocityY = Animator.StringToHash("VelocityY");
            animatorIndex_Die = Animator.StringToHash("Die");
        }
    }

    private void Start()
    {
        // 초기값 전달
        OnGroundChanged?.Invoke();
        OnVelocityChanged?.Invoke();
        OnHpChanged?.Invoke();
    }

    private void FlipSprite()
    {
        if (velocity.x > 0.01f)
            sprite.flipX = false;
        else if (velocity.x < -0.01f)
            sprite.flipX = true;
    }

    private void AnimatorSetGrounded()
    {
        if (isGrounded)
            animator.SetTrigger(animatorIndex_Grounded);
        else
            animator.SetTrigger(animatorIndex_Jump);
    }

    private void AnimatorSetVelocity()
    {
        animator.SetFloat(animatorIndex_VelocityX, velocity.x);
        animator.SetFloat(animatorIndex_VelocityY, velocity.y);
    }

    private void AnimatorSetDie()
    {
        animator.SetTrigger(animatorIndex_Die);
    }
}

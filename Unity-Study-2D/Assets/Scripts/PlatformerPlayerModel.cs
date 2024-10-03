using UnityEngine;
using UnityEngine.Events;

public class PlatformerPlayerModel : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Vector2 velocity;
    [SerializeField] bool isGrounded;

    [Header("Viewer")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;

    private int animatorIndex_IsGrounded;
    private int animatorIndex_VelocityX;
    private int animatorIndex_VelocityY;

    public event UnityAction OnVelocityChanged;
    public event UnityAction OnGroundChanged;

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
            animatorIndex_IsGrounded = Animator.StringToHash("IsGrounded");
            animatorIndex_VelocityX = Animator.StringToHash("VelocityX");
            animatorIndex_VelocityY = Animator.StringToHash("VelocityY");
        }
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
        animator.SetBool(animatorIndex_IsGrounded, isGrounded);
    }

    private void AnimatorSetVelocity()
    {
        animator.SetFloat(animatorIndex_VelocityX, velocity.x);
        animator.SetFloat(animatorIndex_VelocityY, velocity.y);
    }
}

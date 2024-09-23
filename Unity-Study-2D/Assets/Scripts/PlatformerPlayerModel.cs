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
        set { isGrounded = value; OnGroundChanged?.Invoke(); }
    }

    private void Awake()
    {
        if (TryGetComponent(out sprite))
        {
            OnVelocityChanged += FlipSprite;
        }
        if (TryGetComponent(out animator))
        {
            OnGroundChanged += GroundAnimation;
            animatorIndex_IsGrounded = Animator.StringToHash("IsGrounded");
        }
    }

    private void FlipSprite()
    {
        if (velocity.x > 0.01f)
            sprite.flipX = false;
        else if (velocity.x < -0.01f)
            sprite.flipX = true;
    }

    private void GroundAnimation()
    {
        animator.SetBool(animatorIndex_IsGrounded, isGrounded);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class CannonBall : MonoBehaviour
{
    [field: SerializeField]
    public Rigidbody Rigid { get; private set; }

    public UnityEvent<GameObject> OnHitTarget;

    private void OnEnable()
    {
        Rigid.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            OnHitTarget?.Invoke(collision.gameObject);
        }
    }
}

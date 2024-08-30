using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [field: SerializeField] public int Hp { get; set; } = 3;
    public event UnityAction<Target> OnDie;

    private LayerMask bullets;

    private void Awake()
    {
        bullets = LayerMask.NameToLayer("Bullet");
        OnDie += target =>
        {
            Debug.Log("hp zero");
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer == bullets)
        {
            Hp--;
            if (Hp == 0)
                OnDie?.Invoke(this);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour, IDamagedable
{
    [field: SerializeField] public int Hp { get; private set; } = 3;
    public event UnityAction<Target> OnDie;

    private int bulletLayer;

    private void Awake()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
        OnDie += target =>
        {
            Debug.Log("표적 hp 소진");
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer == bulletLayer)
        {
            Damaged(1);
        }
    }

    public void Damaged(int value)
    {
        Hp -= value;
        if (Hp <= 0)
            OnDie?.Invoke(this);
    }

    public void ResetStatus()
    {
        Hp = 3;
    }

    public void Damaged(float damage) => Damaged((int)damage);
}

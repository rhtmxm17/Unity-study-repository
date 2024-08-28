using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float speed;

    public void Shoot(Transform target)
    {
        transform.LookAt(target);
        rigid.velocity = transform.forward * speed;
    }

    public void Shoot(Transform target, float speed)
    {
        this.speed = speed;
        Shoot(target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("적중 확인");
        }

        Destroy(gameObject);
    }
}

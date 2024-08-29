using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [field:SerializeField]
    public Rigidbody Rigid { get; private set; }

    private void OnEnable()
    {
        Rigid.velocity = Vector3.zero;
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] Renderer colorSetRenderer;

    public Color PersonalColor
    { 
        set
        {
            colorSetRenderer.material.color = value;
        }
    }
    public float Damage { get; set; } = 10f;
    public int Id { get; set; }
    public ProjectileShooter Shooter { get; set; }

    // FixedUpdate에서 위치를 갱신시키기 위해 Rigidbody 사용
    private Rigidbody rigid;

    private int hitPossibility = 1;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 velocity)
    {
        rigid.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            Shooter.CallOnHit(this, other.attachedRigidbody.gameObject);
        }
        else
        {
            Shooter.CallOnHit(this, other.gameObject);
        }
    }

    /// <summary>
    /// 적중시 ProjectileShooter의 RPC를 거쳐서 호출될 함수<br/>
    /// 이후 투사체 잔존 여부를 반환
    /// </summary>
    /// <param name="target">적중한 네트워크 오브젝트. 네트워크 오브젝트가 아니라면 null</param>
    /// <returns>true라면 투사체 잔존</returns>
    public bool OnHitGetAlive(PhotonView target)
    {
        hitPossibility--;

        if (target != null && target.IsMine && target.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damaged(Damage);
        }

        return hitPossibility > 0;
    }
}

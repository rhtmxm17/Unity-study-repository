using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastGun : NetworkBehaviour
{
    public LayerMask rayCastLayer;
    public Transform firePose;
    public float Damage = 20f;
    public float range = 50f;

    private void Awake()
    {
        if (firePose == null)
            firePose = Camera.main.transform;
    }

    public void Fire()
    {
        Vector3 rayDir = firePose.forward * range;

        if (Runner.GetPhysicsScene().Raycast(firePose.position, firePose.forward, out RaycastHit hitInfo, range, rayCastLayer))
        {
            Debug.Log($"Hitted:{hitInfo.collider.name}");

            rayDir = hitInfo.point - firePose.position;

            if (hitInfo.rigidbody != null && hitInfo.rigidbody.TryGetComponent(out NetworkedHealthPoint healthPoint))
            {
                Debug.Log($"{Runner.LocalPlayer}에서 DamagedRpc 호출");
                Debug.Log($"타겟 {healthPoint.GetComponent<NetworkObject>().StateAuthority}");
                healthPoint.DamagedRpc(Damage);
            }
        }

        Debug.DrawRay(firePose.position, rayDir, Color.red, 0.5f);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviourPun
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzlePose;

    private Projectile projectileProto; // 원본 프리팹에 개인 설정(색상)을 추가한 프로토타입
    private readonly Projectile[] projectiles = new Projectile[16];

    private void Awake()
    {
        projectileProto = Instantiate(projectilePrefab);
        projectileProto.gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        projectileProto.PersonalColor = color;
    }

    /*
    ViewID 제한: https://doc.photonengine.com/pun/current/gameplay/instantiation#viewid-limits
    보통 플레이어당 PhotonView는 수 개면 충분하고, 총알 등을 모두 PhotonView로 네트워크 인스턴스 하는것은 비효율적이라 한다.
     */
    public void Fire(float velocity)
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (projectiles[i] != null)
                continue;

            photonView.RPC(nameof(FireRPC), RpcTarget.All, i, muzzlePose.position, muzzlePose.forward * velocity);
            return;
        }

        Debug.LogWarning("투사체 관리 공간 부족");
    }

    [PunRPC]
    private void FireRPC(int projectileId, Vector3 fireposition, Vector3 velocity, PhotonMessageInfo info)
    {
        Projectile instance = Instantiate(projectileProto, fireposition, Quaternion.LookRotation(velocity));
        instance.gameObject.SetActive(true);
        projectiles[projectileId] = instance;
        instance.Id = projectileId;
        instance.Shooter = this;
        instance.SetVelocity(velocity);

        // 지연보상
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        instance.GetComponent<Rigidbody>().MovePosition(fireposition + velocity * lag);

        Destroy(instance.gameObject, 3f - lag);
    }

    /*
     적중시 데미지 및 소멸 처리하기
    
    마스터 클라이언트에서 타격 판정을 일괄로 관리한다면 크게 문제가 없을 것 같다
    피격 판정을 각 클라이언트가 수행해야 한다면 소유권이 다른 두 캐릭터가 거의 동시에 피격시 문제가 생길듯
    ViaServer를 사용해 해당 내용이 연속으로 호출되더라도 가장 먼저 호출된 것만 실제로 적용되게 하자
     */
    public void CallOnHit(Projectile source, GameObject target)
    {
        Debug.Log($"적중 대상: {target.name}");
        bool found = target.TryGetComponent(out PhotonView targetView);
        int targetId = 0;
        if (targetView != null)
        {
            if (false == targetView.IsMine)
            {
                Debug.Log("소유권이 없는 대상의 피격 판정 무시");
                return;
            }
            targetId = targetView.ViewID;
        }

        photonView.RPC(nameof(OnHitRPC), RpcTarget.AllViaServer, source.Id, found, targetId);
    }

    [PunRPC]
    private void OnHitRPC(int sourceId, bool targetIsPhotonView, int targetID)
    {
        Projectile source = projectiles[sourceId];
        if (source == null || source.isActiveAndEnabled == false)
            return;

        PhotonView target = targetIsPhotonView ? PhotonView.Find(targetID) : null;
        Debug.Log($"RPC에서 확인된 적중 대상: {targetIsPhotonView} / {targetID}");

        bool isAlive = source.OnHitGetAlive(target);

        if (isAlive == false)
        {
            source.gameObject.SetActive(false);
            Destroy(source.gameObject, 1f);
        }
    }
}

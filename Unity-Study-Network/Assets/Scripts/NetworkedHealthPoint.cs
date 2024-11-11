using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkedHealthPoint : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnHpChangedRender))]
    public float NetworkHp { get; set; }

    public UnityEvent<float> OnHpChanged;
    public UnityEvent OnDead;

    private void OnHpChangedRender()
    {
        OnHpChanged?.Invoke(NetworkHp);

        if (NetworkHp <= 0f)
        {
            OnDead?.Invoke();
        }    
    }


    public float MaxHp { get; set; } = 100f;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            NetworkHp = MaxHp;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DamagedRpc(float damage)
    {
        if (NetworkHp <= 0f)
        {
            Debug.Log("이미 HP가 소진된 상태에서 피해 발생");
            return;
        }

        Debug.Log($"{Runner.LocalPlayer} 받은 피해 {damage}");
        NetworkHp -= damage;
    }
}

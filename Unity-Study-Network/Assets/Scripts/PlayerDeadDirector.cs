using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(NetworkedHealthPoint))]
public class PlayerDeadDirector : MonoBehaviour
{
    /**
     * 플레이어 사망시 처리
     * 공통: 굴러다니게 만들기, 피격 비활성화
     * 소유권자: 입력 비활성화, 카메라 처리 <- 컨트롤 관련이니 Controller에서?
     */

    [SerializeField] Collider hitBox;
    private NetworkedHealthPoint healthPoint;

    private void Awake()
    {
        healthPoint = GetComponent<NetworkedHealthPoint>();
        healthPoint.OnDead.AddListener(DeadDirect);
    }


    private void DeadDirect()
    {
        if (hitBox != null)
        {
            hitBox.gameObject.layer = LayerMask.NameToLayer("Physics Effect");
        }

        if (TryGetComponent(out CharacterController controller))
        {
            controller.enabled = false;
        }

        if (TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
        }
    }
}

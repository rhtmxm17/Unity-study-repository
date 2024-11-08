using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public Vector3 lookAt = Vector3.zero; // target�� space ���� �ٶ� ��ǥ(ex �Ӹ� ��ġ)
    [field: SerializeField] public Vector3 lookFrom; // lookAt ������ ī�޶� space ���� ��� �� ������

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 at = Target.transform.TransformPoint(lookAt);
        Vector3 from = at - this.transform.TransformVector(lookFrom);

        transform.position = from;
    }
}

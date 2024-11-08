using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public Vector3 lookAt = Vector3.zero; // target의 space 기준 바라볼 좌표(ex 머리 위치)
    [field: SerializeField] public Vector3 lookFrom; // lookAt 지점을 카메라 space 기준 어디에 둘 것인지

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 at = Target.transform.TransformPoint(lookAt);
        Vector3 from = at - this.transform.TransformVector(lookFrom);

        transform.position = from;
    }
}

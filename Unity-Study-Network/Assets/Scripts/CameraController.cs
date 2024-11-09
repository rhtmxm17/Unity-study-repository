using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public Vector3 lookAt = Vector3.zero; // target의 space 기준 바라볼 좌표(ex 머리 위치)
    [field: SerializeField] public Vector3 lookFrom; // lookAt 지점을 카메라 space 기준 어디에 둘 것인지

    [SerializeField] Vector2 cameraSensitivity = Vector3.one * 0.3f;
    [SerializeField] float maxVerticalCameraAngle = 60f;

    private InputAction lookInput;
    float verticalCameraAngle;
    float horizontalCameraAngle;

    private void Awake()
    {
        lookInput = PlayerInput.GetPlayerByIndex(0).actions["Look"];
    }

    private void Update()
    {
        Vector2 cursorDelta = lookInput.ReadValue<Vector2>();

        horizontalCameraAngle += cursorDelta.x * cameraSensitivity.x;
        verticalCameraAngle += cursorDelta.y * cameraSensitivity.y;
        verticalCameraAngle = Mathf.Clamp(verticalCameraAngle, -maxVerticalCameraAngle, maxVerticalCameraAngle);

        transform.rotation = Quaternion.Euler(-verticalCameraAngle, horizontalCameraAngle, 0f);
    }

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 at = Target.transform.TransformPoint(lookAt);
        Vector3 from = at - this.transform.TransformVector(lookFrom);

        transform.position = from;
    }
}

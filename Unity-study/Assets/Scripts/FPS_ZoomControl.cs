using Cinemachine;
using UnityEngine;

public class FPS_ZoomControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera zoomCamera;
    [SerializeField] private AxisInputRotator rotator;
    [SerializeField] private int togglePriorityValue = 10;
    [SerializeField] private float toggleSpeedGain = 0.5f;
    [SerializeField] private KeyCode toggleKey = KeyCode.Mouse1;
    [Space]
    [SerializeField] private ShooterPlayerModel model;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            model.Zoom = true;
            zoomCamera.Priority += togglePriorityValue;
            rotator.SpeedGain = toggleSpeedGain;
        }
        if (Input.GetKeyUp(toggleKey))
        {
            model.Zoom = false;
            zoomCamera.Priority -= togglePriorityValue;
            rotator.SpeedGain = 1f;
        }
    }
}

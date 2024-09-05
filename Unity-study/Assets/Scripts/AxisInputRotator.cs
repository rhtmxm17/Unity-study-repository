using UnityEngine;

public class AxisInputRotator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Tooltip("카메라 회전 계수. Inspector에서 수정시 오작동할 수 있음")] private float speedGain = 1f;
    public float SpeedGain
    {
        get => speedGain;
        set
        {
            float gain = value / speedGain;
            vertical.speed *= gain;
            horizontal.speed *= gain;
            speedGain = value;
        }
    }

    [SerializeField]
    private AxisInfo vertical = new AxisInfo()
    {
        valueRangeMin = -70f,
        valueRangeMax = 70f,
        wrap = false,
        speed = 5f,
        inputAxisName = "Mouse Y",
        invert = true,
    };

    [SerializeField]
    private AxisInfo horizontal = new AxisInfo()
    {
        valueRangeMin = -180f,
        valueRangeMax = 180f,
        wrap = true,
        speed = 5f,
        inputAxisName = "Mouse X",
        invert = false,
    };

    // CinemachineVirtualCamera의 POV 조준모드를 참고해서 구성함
    [System.Serializable]
    private struct AxisInfo
    {
        // 매 프레임 사용되는 값
        public float value;
        public float inputAxisValue;

        // 설정값
        public float valueRangeMin;
        public float valueRangeMax;
        public bool wrap;
        public float speed;

        public string inputAxisName;
        public bool invert;

        public void UpdateValue()
        {
            inputAxisValue = Input.GetAxis(inputAxisName);
            value += inputAxisValue * speed * (invert ? -1f : 1f);
            if (false == wrap)
                value = Mathf.Clamp(value, valueRangeMin, valueRangeMax);
        }
    }

    private void Update()
    {
        horizontal.UpdateValue();
        vertical.UpdateValue();

        target.localEulerAngles = new Vector3(vertical.value, horizontal.value, 0f);
    }
}

using UnityEngine;

public class HelicopterControl : MonoBehaviour
{
    public float PropellerSpeed
    {
        get => propellerSpeed;
        set
        {
            if (value < 0f)
                propellerSpeed = 0f;
            else if (value > maxRotationSpeed)
                propellerSpeed = maxRotationSpeed;
            else
                propellerSpeed = value;
        }
    }

    [Header("프로펠러")]

    [SerializeField] private Transform propellerTransform;
    [SerializeField] private float propellerSpeed = 0f;
    [SerializeField] private float maxRotationSpeed = 720f;
    [SerializeField] private float propellerAcceleration = 180f;
    [SerializeField] private float takeoff = 360f;
    [SerializeField] private float ascendingSpeed = 8f;

    [Header("전후좌우")]

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 45f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 프로펠러 회전속도 갱신
        if (Input.GetButton("Jump"))
        {
            PropellerSpeed += propellerAcceleration * Time.deltaTime;
        }
        else
        {
            PropellerSpeed -= propellerAcceleration * Time.deltaTime;
        }

        // 고도 갱신
        if (propellerSpeed > takeoff)
            transform.Translate(Vector3.up * ascendingSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.up * -ascendingSpeed * Time.deltaTime);


        if (transform.position.y < 0f)
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        if (transform.position.y > 50f)
            transform.position = new Vector3(transform.position.x, 50f, transform.position.z);


        // 프로펠러 회전
        propellerTransform.Rotate(Vector3.up, PropellerSpeed * Time.deltaTime);


        // 전후
        float vertical = moveSpeed * Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * vertical * Time.deltaTime, Space.Self);

        // 좌우
        float horizontal = rotationSpeed * Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * Time.deltaTime);
    }

}

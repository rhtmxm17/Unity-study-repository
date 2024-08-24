using UnityEngine;

public class HelicopterControl : MonoBehaviour
{
    public float PropellerRotatePerSecond
    {
        get => propellerRotatePerSecond;
        set
        {
            if (value < 0f)
                propellerRotatePerSecond = 0f;
            else if (value > maxRPS)
                propellerRotatePerSecond = maxRPS;
            else
                propellerRotatePerSecond = value;
        }
    }

    [Header("프로펠러")]

    [SerializeField] private Transform propellerTransform;
    [SerializeField] private float propellerRotatePerSecond = 0f;
    [SerializeField] private float maxRPS = 2f;
    [SerializeField] private float propellerRPS_Acceleration = 0.5f;

    [Header("상승하강")]

    [SerializeField] private float takeoffRPS = 1f;
    [SerializeField] private float ascendingSpeedPerRPS = 2f;
    [SerializeField] private float maxAscending = 10f;
    [SerializeField] private float minAscending = -10f;

    [Header("전후좌우")]

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 45f;

    // Update is called once per frame
    void Update()
    {
        // 프로펠러 회전속도 갱신
        if (Input.GetButton("Jump"))
        {
            PropellerRotatePerSecond += propellerRPS_Acceleration * Time.deltaTime;
        }
        else
        {
            PropellerRotatePerSecond -= propellerRPS_Acceleration * Time.deltaTime;
        }

        // 고도 갱신
        float ascending = (propellerRotatePerSecond - takeoffRPS) * ascendingSpeedPerRPS;

        if (ascending < minAscending)
            ascending = minAscending;

        if (ascending > maxAscending)
            ascending = maxAscending;

        transform.Translate(Vector3.up * ascending * Time.deltaTime);

        if (transform.position.y < 0f)
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        if (transform.position.y > 50f)
            transform.position = new Vector3(transform.position.x, 50f, transform.position.z);


        // 프로펠러 회전
        propellerTransform.Rotate(Vector3.up, PropellerRotatePerSecond * 360f * Time.deltaTime);


        // 전후
        float vertical = moveSpeed * Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * vertical * Time.deltaTime, Space.Self);

        // 좌우
        float horizontal = rotationSpeed * Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * Time.deltaTime);
    }

}

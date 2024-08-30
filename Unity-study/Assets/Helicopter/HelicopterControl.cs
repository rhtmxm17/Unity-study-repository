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
    [SerializeField] private float maxRPS = 10f;
    [SerializeField] private float propellerRPS_Acceleration = 1f;
    [SerializeField] private float fuel = 30f;

    [Header("상승하강")]

    [SerializeField] private float takeoffRPS = 5f;
    [SerializeField] private float ascendingSpeedPerRPS = 2f;
    [SerializeField] private float maxAscending = 10f;
    [SerializeField] private float minAscending = -10f;

    [Header("전후좌우")]

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 45f;

    [Header("미사일")]


    [SerializeField] private GameObject missleOrigine;
    [SerializeField] private float shootCoolDown = 1f;
    [SerializeField] private bool shootWhileFlyingOnly = false;

    private float coolDownProgress = 0f;
    private GameObject[] missles = new GameObject[4];
    private int nextShoot = 0;

    private void Start()
    {
        if (missleOrigine.GetComponent<Missile>() is null)
            Debug.LogError("missleOrigine에 설정된 오브젝트가 Missile을 갖고있지 않습니다.");

        for (int i = 0; i < missles.Length; i++)
        {
            missles[i] = Instantiate(missleOrigine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        ShootCheck();
    }

    private void UpdateMovement()
    {
        // 프로펠러 회전속도 갱신
        if (Input.GetButton("Jump") && (fuel > 0f))
        {
            PropellerRotatePerSecond += propellerRPS_Acceleration * Time.deltaTime;
            fuel -= Time.deltaTime;
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

    private void ShootCheck()
    {
        coolDownProgress -= Time.deltaTime;

        if (shootWhileFlyingOnly && transform.position.y <= 0f)
            return;

        if (false == Input.GetButton("Fire3")
            || coolDownProgress > 0f
            || missles[nextShoot].activeSelf)
            return;

        coolDownProgress = shootCoolDown;

        missles[nextShoot].GetComponent<Missile>().ShootSetting(20f, transform.position, transform.rotation);
        nextShoot++;
        if (nextShoot >= 4)
            nextShoot = 0;
    }
}

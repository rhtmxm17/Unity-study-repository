using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuikaShooter : MonoBehaviour
{
    [SerializeField] private SuikaBall suikaBallPrefab;
    [SerializeField] private Transform charge;
    [SerializeField] private float powerCoefficient = 5f;
    [SerializeField] private float chargeUI_coefficient = 0.1f;
    private PlayerInput playerInput;
    private Coroutine lookAtMouseRoutine;
    private float clickTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions["Click"].started += context =>
        {
            clickTime = Time.time;
            lookAtMouseRoutine = StartCoroutine(LookAtMouse());
        };

        playerInput.actions["Click"].canceled += context =>
        {
            StopCoroutine(lookAtMouseRoutine);
            ThrowSuika((float)(Time.time - clickTime));
            charge.localPosition = Vector3.zero;
        };
    }

    private IEnumerator LookAtMouse()
    {
        YieldInstruction wait = null;
        while (true)
        {
            Vector2 cursor = playerInput.actions["Point"].ReadValue<Vector2>();
            Vector3 cursorInWorld = Camera.main.ScreenToWorldPoint(cursor);
            cursorInWorld.z = transform.position.z;

            transform.right = cursorInWorld - transform.position;
            charge.localPosition = powerCoefficient * chargeUI_coefficient * (Time.time - clickTime) * Vector3.left;

            yield return wait;
        }
    }

    private void ThrowSuika(float power)
    {
        Debug.Log(power);
        var suika = Instantiate(suikaBallPrefab, transform.position, Quaternion.identity);
        suika.GetComponent<Rigidbody2D>().AddForce(powerCoefficient * power * transform.right, ForceMode2D.Impulse);
    }
}

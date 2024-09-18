using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuikaShooter : MonoBehaviour
{
    [SerializeField] private SuikaBall suikaBallPrefab;
    [SerializeField] private float powerCoefficient = 5f;
    private PlayerInput playerInput;
    private Coroutine lookAtMouseRoutine;
    private double clickTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions["Click"].started += context =>
        {
            clickTime = context.time;
            lookAtMouseRoutine = StartCoroutine(LookAtMouse());
        };

        playerInput.actions["Click"].canceled += context =>
        {
            StopCoroutine(lookAtMouseRoutine);
            ThrowSuika((float)(context.time - clickTime));
        };
    }

    private IEnumerator LookAtMouse()
    {
        YieldInstruction wait = new WaitForFixedUpdate();
        while (true)
        {
            Vector2 cursor = playerInput.actions["Point"].ReadValue<Vector2>();
            Vector3 cursorInWorld = Camera.main.ScreenToWorldPoint(cursor);
            cursorInWorld.z = transform.position.z;

            transform.right = cursorInWorld - transform.position;

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

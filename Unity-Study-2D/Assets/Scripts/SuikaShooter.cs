using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuikaShooter : MonoBehaviour
{
    [SerializeField] private SuikaBall suikaBallPrefab;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions["Click"].performed += context =>
        {
            StartCoroutine(LookAtMouse());
        };
    }

    private IEnumerator LookAtMouse()
    {
        WaitForFixedUpdate wait = new();
        //while (true)
        {
            Vector2 cursor = playerInput.actions["Point"].ReadValue<Vector2>();
            Vector2 cursorInWorld = Camera.main.ScreenToWorldPoint(cursor);

            Debug.Log(cursor);
            Debug.Log(cursorInWorld);

            yield return wait;
        }
    }
}

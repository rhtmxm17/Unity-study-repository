using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActivateToggleInput : MonoBehaviour
{
    [SerializeField] private GameObject[] targetObjects;
    [SerializeField] private Behaviour[] targetBehaviour;
    [SerializeField] private KeyCode toggleKey;
    [SerializeField] private bool keyDownIsEnable;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            foreach (var obj in targetObjects)
            {
                obj.SetActive(keyDownIsEnable);
            }
            foreach (var obj in targetBehaviour)
            {
                obj.enabled = keyDownIsEnable;
            }
        }
        if (Input.GetKeyUp(toggleKey))
        {
            foreach (var obj in targetObjects)
            {
                obj.SetActive(!keyDownIsEnable);
            }
            foreach (var obj in targetBehaviour)
            {
                obj.enabled = !keyDownIsEnable;
            }
        }
    }
}

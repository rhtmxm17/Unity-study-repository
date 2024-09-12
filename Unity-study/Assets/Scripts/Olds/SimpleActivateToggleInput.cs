using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActivateToggleInput : MonoBehaviour
{
    [SerializeField] private GameObject[] targetObjects;
    [SerializeField] private Behaviour[] targetBehaviour;
    [SerializeField] private KeyCode toggleKey;
    [SerializeField, Tooltip("True: 스위치, False: 버튼")] private bool isSwitch;
    [SerializeField, Tooltip("버튼일때만 유효")] private bool keyDownIsEnable;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            foreach (var obj in targetObjects)
            {
                obj.SetActive(isSwitch ? !obj.activeSelf : keyDownIsEnable);
            }
            foreach (var obj in targetBehaviour)
            {
                obj.enabled = isSwitch ? !obj.enabled : keyDownIsEnable;
            }
        }
        if (Input.GetKeyUp(toggleKey) && !isSwitch)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    [SerializeField] private GameObject mapCamera;

    private void Start()
    {
        mapCamera = GameObject.FindWithTag("MapCamera");
    }

    private void LateUpdate()
    {
        gameObject.transform.rotation = mapCamera.transform.rotation;
    }
}

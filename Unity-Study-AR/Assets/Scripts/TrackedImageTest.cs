using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImage))]
public class TrackedImageTest : MonoBehaviour
{
    ARTrackedImage image;

    private void Awake()
    {
        image = GetComponent<ARTrackedImage>();
    }

    private void Start()
    {
        Debug.Log($"[TrackedImageTest] {image.referenceImage.name} 생성됨");
    }
}

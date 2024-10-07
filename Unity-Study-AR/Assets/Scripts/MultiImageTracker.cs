using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MultiImageTracker : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
        {
            // 이미지를 구분하는데 사용
            string name = image.referenceImage.name;
            Debug.Log($"[MultiImageTracker] TrackedImage {image.referenceImage.name} 추가됨");

            Instantiate(prefab, image.transform);
        }

        foreach (ARTrackedImage image in args.updated)
        {
            // 이미지의 상태가 변경되었을 때
        }

        foreach (ARTrackedImage image in args.removed)
        {
            Debug.Log($"[MultiImageTracker] TrackedImage {image.referenceImage.name} 제거됨");
        }
    }
}

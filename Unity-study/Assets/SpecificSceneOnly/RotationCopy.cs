using UnityEngine;

public class RotationCopy : MonoBehaviour
{
    [SerializeField] private Transform source;
    [SerializeField] private Transform target = null;

    private void Awake()
    {
        if (target == null)
            target = transform;
    }

    private void LateUpdate()
    {
        target.transform.rotation = source.rotation;
    }
}

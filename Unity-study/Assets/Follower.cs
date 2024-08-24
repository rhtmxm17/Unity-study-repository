using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 interval;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(target);
        transform.SetLocalPositionAndRotation(interval, transform.rotation);
    }
}

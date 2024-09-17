using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePhysicsSetting : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.gravity = Vector2.zero;
        Destroy(gameObject);
    }
}

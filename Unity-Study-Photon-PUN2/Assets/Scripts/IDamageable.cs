using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public GameObject gameObject { get; }

    public void Damaged(float damage);
}

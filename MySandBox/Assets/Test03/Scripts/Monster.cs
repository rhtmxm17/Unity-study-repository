using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test03
{
    public class Monster : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
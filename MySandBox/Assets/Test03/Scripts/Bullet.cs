using System.Collections;
using UnityEngine;

namespace Test03
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
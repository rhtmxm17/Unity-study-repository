using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 lookAt = Vector3.zero;
    [SerializeField] private Vector3 lookFrom;

    private void LateUpdate()
    {
        Quaternion rotationY = Quaternion.AngleAxis(target.rotation.eulerAngles.y, Vector3.up);
        Vector3 from = rotationY * lookFrom + target.position;
        Vector3 at = rotationY * lookAt + target.position;

        transform.position = from;
        transform.LookAt(at);
    }
}

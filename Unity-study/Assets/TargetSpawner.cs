using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Target protoTarget;
    [SerializeField] private Vector3[] respawnArea = new Vector3[2]; //Bounds struct
    [SerializeField] private int targetNumber = 10;

    [SerializeField] private float respawnTime = 2f;
    private WaitForSeconds wait;

    private Vector3 gizmoCenter;
    private Vector3 gizmoSize;

    private void Awake()
    {
        wait = new WaitForSeconds(respawnTime);
        gizmoCenter = (respawnArea[0] + respawnArea[1]) * 0.5f;
        gizmoSize = (respawnArea[1] - respawnArea[0]);

        for (int i = 0; i < targetNumber; i++)
        {
            Target instance = Instantiate(protoTarget, GetRespawnPoint(), Random.rotation);
            instance.OnDie += target =>
            {
                target.gameObject.SetActive(false);
                StartCoroutine(Respawn(target));
            };
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gizmoCenter, gizmoSize);
    }

    private Vector3 GetRespawnPoint()
    {
        return new Vector3
        (
            Random.Range(respawnArea[0].x, respawnArea[1].x),
            Random.Range(respawnArea[0].y, respawnArea[1].y),
            Random.Range(respawnArea[0].z, respawnArea[1].z)
        );
    }

    private IEnumerator Respawn(Target target)
    {
        yield return wait;

        target.ResetStatus();
        target.gameObject.SetActive(true);
        target.transform.SetPositionAndRotation(GetRespawnPoint(), Random.rotation);
    }

}

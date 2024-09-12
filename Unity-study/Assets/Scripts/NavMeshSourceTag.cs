using UnityEngine;
using UnityEngine.AI;

public class NavMeshSourceTag : MonoBehaviour
{
    [SerializeField] private NavMeshBuildData sharedData;

    public NavMeshBuildSource GetBuildSource() => sharedData.GetBuildSource(transform);
}

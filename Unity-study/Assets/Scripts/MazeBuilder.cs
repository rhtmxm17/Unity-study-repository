using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeBuilder : MonoBehaviour
{
    [SerializeField] private MazeData mazeData;
    [SerializeField] private NavMeshSourceTag[] extraSource;

    private GameObject[,] blocks;
    private NavMeshDataInstance navInstance;

    private void Awake()
    {
        blocks = new GameObject[mazeData.MapSize.x, mazeData.MapSize.y];
    }

    private void Start()
    {
        BuildWallAndNavMesh();
        StartAnimation();
    }

    private void BuildWallAndNavMesh()
    {

        List<NavMeshBuildSource> buildSources = new();
        Vector3 leftTop = new Vector3(mazeData.MapSize.x - 1, 0, mazeData.MapSize.y - 1) * -0.5f;

        foreach (var source in extraSource)
        {
            buildSources.Add(source.GetBuildSource());
        }

        foreach (var elements in mazeData.Elements)
        {
            foreach (var position in elements.positions)
            {
                GameObject instance = Instantiate(elements.prefab, transform);
                instance.transform.SetLocalPositionAndRotation(new Vector3(position.x, 0, position.y) + leftTop, transform.rotation);
                instance.SetActive(false);
                blocks[position.x, position.y] = instance;

                // 태그를 달고있다면 빌드 정보 등록
                if (instance.TryGetComponent(out NavMeshSourceTag tag))
                {
                    buildSources.Add(tag.GetBuildSource());
                }
            }

        }


        //// 에디터 한정 네임스페이스, 빌드된 게임에서는 못쓰는듯..
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

        // 기존 Agents 정보 가져오기
        NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(0);

        // 데이터 생성
        NavMeshData data = NavMeshBuilder.BuildNavMeshData(settings, buildSources, new Bounds(Vector3.zero, Vector3.one * 10f), Vector3.zero, Quaternion.identity);

        // Bake된 기존 데이터 제거
        // 아예 Bake 안하면 어째선지 유효하지 않다고 나옴...
        NavMesh.RemoveAllNavMeshData();

        // 생성한 데이터 넣기
        navInstance = NavMesh.AddNavMeshData(data);
    }

    private void StartAnimation()
    {
        for (int x = 0; x < mazeData.MapSize.x; x++)
        {
            for (int y = 0; y < mazeData.MapSize.y; y++)
            {
                if (blocks[x, y] != null)
                {
                    StartCoroutine(TileActiveRoutine(blocks[x, y], (x + y) * 0.1f));
                }
            }
        }
    }

    private IEnumerator TileActiveRoutine(GameObject tile, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tile.SetActive(true);
    }
}

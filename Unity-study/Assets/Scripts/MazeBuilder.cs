using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeBuilder : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject plane;

    private GameObject[,] tiles;
    private NavMeshDataInstance navInstance;

    private void Awake()
    {
        tiles = new GameObject[8, 8];
    }

    private void Start()
    {
        BuildWallAndNavMesh();
        StartAnimation();
    }

    private void BuildWallAndNavMesh()
    {
        // Bake된 기존 데이터 제거
        // 아예 Bake 안하면 어째선지 유효하지 않다고 나옴...
        NavMesh.RemoveAllNavMeshData();

        List<NavMeshBuildSource> buildSources = new(12);
        buildSources.Add(new NavMeshBuildSource()
        {
            area = 0, // Walkable
            shape = NavMeshBuildSourceShape.Mesh,
            transform = plane.transform.localToWorldMatrix,
            size = Vector3.one,
            component = plane.GetComponent<MeshFilter>(),
            sourceObject = plane.GetComponent<MeshFilter>().sharedMesh,
        });

        // 벽 생성
        for (int i = 0; i < 10; i++)
        {
            int x, y;
            do
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
            } while (tiles[x, y] != null);

            tiles[x, y] = Instantiate(wallPrefab, transform);
            tiles[x, y].transform.SetLocalPositionAndRotation(new Vector3(x, 0, y), transform.rotation);
            tiles[x, y].SetActive(false);

            buildSources.Add(new NavMeshBuildSource()
            {
                area = 1, // Not Walkable
                shape = NavMeshBuildSourceShape.Box,
                transform = tiles[x, y].transform.localToWorldMatrix,
                size = Vector3.one,
                component = tiles[x, y].GetComponentInChildren<MeshFilter>(),
                sourceObject = tiles[x, y].GetComponentInChildren<MeshFilter>().sharedMesh,
            });
        }

        // 물 생성
        for (int i = 0; i < 6; i++)
        {
            int x, y;
            do
            {
                x = Random.Range(0, 8);
                y = Random.Range(0, 8);
            } while (tiles[x, y] != null);

            tiles[x, y] = Instantiate(waterPrefab, transform);
            tiles[x, y].transform.SetLocalPositionAndRotation(new Vector3(x, 0, y), transform.rotation);
            tiles[x, y].SetActive(false);

            buildSources.Add(new NavMeshBuildSource()
            {
                area = 3, // Water
                shape = NavMeshBuildSourceShape.ModifierBox,
                transform = tiles[x, y].transform.localToWorldMatrix,
                size = Vector3.one,
                component = tiles[x, y].GetComponentInChildren<MeshFilter>(),
                sourceObject = tiles[x, y].GetComponentInChildren<MeshFilter>().sharedMesh,
            });
        }

        //// 에디터 한정 네임스페이스, 빌드된 게임에서는 못쓰는듯..
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

        // 기존 Agents 정보 가져오기
        NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(0);

        // 데이터 생성
        NavMeshData data = NavMeshBuilder.BuildNavMeshData(settings, buildSources, new Bounds(Vector3.zero, Vector3.one * 10f), Vector3.zero, Quaternion.identity);

        // 생성한 데이터 넣기
        navInstance = NavMesh.AddNavMeshData(data);
    }

    private void StartAnimation()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (tiles[x, y] != null)
                {
                    StartCoroutine(TileActiveRoutine(tiles[x, y], (x + y) * 0.1f));
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

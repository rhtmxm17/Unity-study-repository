using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class TileMapAstar : MonoBehaviour
{
    [SerializeField] Tilemap wall;

    private AStarMap map;
    private int version = 0;

    private static Vector2Int[] directions =
    {
        new(0, 1), // 상
        new(0, -1), // 하
        new(1, 0), // 우
        new(-1, 0), // 좌
    };

    private class AStarMap
    {
        private AStarNode[,] nodes;
        private Vector2Int originPosition;

        public ref AStarNode this[Vector2Int position]
        {
            get
            {
                position -= originPosition;
                return ref nodes[position.x, position.y];
            }
        }

        public AStarMap(Tilemap map)
        {
            originPosition = (Vector2Int)map.origin;
            nodes = new AStarNode[map.size.x, map.size.y];
        }
    }

    private struct AStarNode : IComparable<AStarNode>
    {
        public int openedVersion;
        public Vector2Int parent;
        public bool isOpened;
        public int f { get => g + h; }
        public int g;
        public int h;

        public int CompareTo(AStarNode other)
        {
            int compF = f.CompareTo(other.f);
            if (compF != 0)
                return compF;
            else
                return h.CompareTo(other.h);
        }
    }

    // 예상 거리 비교자
    private class AstarDesending : IComparer<Vector2Int>
    {
        private AStarMap map;

        public AstarDesending(AStarMap map)
        {
            this.map = map;
        }

        // 내림차순
        public int Compare(Vector2Int left, Vector2Int right)
        {
            return -map[left].CompareTo(map[right]);
        }
    }

    private void Awake()
    {
        map = new(wall);
    }

    private void Start()
    {
        FindPath(new Vector2Int(-3, 2), new Vector2Int(4, -3), out var path);

        foreach(var item in path)
        {
            Debug.Log(item);
        }
    }

    public bool FindPath(Vector2Int from, Vector2Int to, out List<Vector2Int> path)
    {
        version++;

        // 삽입시 우선순위 내림차순 정렬
        // 삭제시 마지막 요소를 꺼내기
        //  ->우선순위 큐로 사용
        List<Vector2Int> openNodes = new List<Vector2Int>();
        AstarDesending astarDesending = new AstarDesending(map);

        // 초기 데이터 설정
        openNodes.Add(from);
        map[from].openedVersion = version;
        map[from].g = 0;

        while (openNodes.Count > 0)
        {
            Vector2Int peek = openNodes[openNodes.Count - 1];
            openNodes.RemoveAt(openNodes.Count - 1);

            int nextG = map[peek].g + 10;

            Debug.Log($"[{peek}] g:{map[peek].g} h:{map[peek].h}");

            for (int i = 0; i < directions.Length; i++)
            {
                Vector2Int near = peek + directions[i];
                Vector3Int v3near = (Vector3Int)near;

                // 범위 검사
                if (false == wall.cellBounds.Contains(v3near))
                    continue;

                // 벽 검사
                if (wall.HasTile(v3near))
                    continue;

                // 이미 열렸는지 검사
                if (map[near].openedVersion == version)
                {
                    // 새로 발견한 경로가 더 짧다면 갱신
                    if (map[near].g > nextG)
                    {
                        map[near].parent = peek;
                        map[near].g = nextG;
                    }
                    continue;
                }

                // 데이터 입력
                map[near].openedVersion = version;
                map[near].parent = peek;
                map[near].g = nextG;
                map[near].h = Distance(near, to);

                // 도착 검사
                if (near == to)
                    break;

                // 정렬해서 List에 추가
                int addTo = openNodes.BinarySearch(near, astarDesending);
                if (addTo < 0)
                    addTo = ~addTo;
                
                openNodes.Insert(addTo, near);
            }
        }

        if (map[to].openedVersion != version)
        {
            path = null;
            return false;
        }

        path = new List<Vector2Int>(map[to].g >> 3);
        Vector2Int parent = to;
        while (parent != from)
        {
            path.Add(parent);
            parent = map[parent].parent;
        }
        path.Reverse();

        return true;
    }

    private static int Distance(Vector2Int left, Vector2Int right)
    {
        Vector2Int dist = left - right;
        return 10 * (Mathf.Abs(dist.x) + Mathf.Abs(dist.y));
    }
}

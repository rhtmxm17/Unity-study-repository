using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Maze Data")]
public class MazeData : ScriptableObject
{
    [System.Serializable]
    public struct MazeElements
    {
        public string id;
        public GameObject prefab;
        public Vector2Int[] positions;
    }

    [SerializeField] private MazeElements[] elements = Array.Empty<MazeElements>();
    [SerializeField] private Vector2Int mapSize;

    public MazeElements[] Elements { get => elements; }

    public Vector2Int MapSize { get => mapSize; }

#if UNITY_EDITOR // csv 파일로부터 데이터 가져오기
    private struct ElementLoadData
    {
        public GameObject prefab;
        public List<Vector2Int> positions;
    }

    [ContextMenu("Create From CSV"), Tooltip("같은 이름의 .csv 파일로부터 생성")]
    private void CreateFromCSV()
    {
        Debug.Log($"현재 System.IO 상대경로 기본 위치: {Path.GetFullPath(".")}");
        string selfPath = AssetDatabase.GetAssetPath(this);
        string csvPath = Path.ChangeExtension(selfPath, ".csv"); // 동명의 확장자만 다른 csv파일 경로
        
        if (false == File.Exists(csvPath))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
        }

        // 읽어온 데이터를 가변 크기로 담아둘 임시 공간
        Dictionary<string, ElementLoadData> dataFromCsv = new();

        string allText = File.ReadAllText(csvPath).TrimEnd();
        string[] lines = allText.Split('\n');

        mapSize.y = lines.Length;
        for (int y = 0; y < lines.Length; y++)
        {
            string[] cell = lines[y].TrimEnd().Split(',');

            mapSize.x = mapSize.x > cell.Length ? mapSize.x : cell.Length;
            for (int x = 0; x < cell.Length; x++)
            {
                // 각 셀 마다 정보 저장
                if (cell[x] == string.Empty)
                    continue;

                if (dataFromCsv.ContainsKey(cell[x]))
                {
                    // 이미 추가된 속성이라면 Dictionary.value에 좌표만 추가
                    dataFromCsv[cell[x]].positions.Add(new Vector2Int(x, y));
                }
                else
                {
                    // 기존 데이터에 같은 키값으로 등록된 prefab이 있다면 기록한다
                    int find = ArrayUtility.FindIndex(elements, element =>
                    {
                        return element.id == cell[x];
                    });
                    GameObject oldPrefab = find < 0 ? null : elements[find].prefab;

                    // 처음 발견한 키값이라면 Dictionary.Add
                    dataFromCsv.Add(cell[x], new ElementLoadData()
                    {
                        prefab = oldPrefab,
                        positions = new List<Vector2Int>() { new Vector2Int(x, y) },
                    });
                }
            }
        }

        // 읽어온 데이터를 배열로 변환
        elements = new MazeElements[dataFromCsv.Count];
        int i = 0;
        foreach (var pair in dataFromCsv)
        {
            elements[i] = new MazeElements()
            {
                id = pair.Key,
                prefab = pair.Value.prefab,
                positions = pair.Value.positions.ToArray(),
            };
            i++;
        }

        // 완료
        Debug.LogWarning("읽어오기 완료, ID별 비어있는 GameObject는 수동으로 넣어주세요");

        //MazeData newData = CreateInstance<MazeData>();
        //AssetDatabase.CreateAsset(newData, $"Assets/Datatables/MazeMaps/MazeDataFromCSV.asset");
    }
#endif
}

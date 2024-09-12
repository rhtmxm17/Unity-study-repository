using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVParseSample : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponData
    {
        public int index;
        public string name;
        public int atk;
        public int dfe;
        public string description;
    }

    [SerializeField] WeaponData[] inspectorView;
    private Dictionary<int, WeaponData> weaponsData;

    private void Awake()
    {
        // 에디터에서는 Assets 경로
        // 환경에 따라서 읽기 전용이기때문에 저장에는 쓰지 않는다고 생각
        Debug.Log(Application.dataPath);

        // Windows라면 User\AppData\ 아래의 로컬 저장소. 쓰기 가능
        Debug.Log(Application.persistentDataPath);

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        // Windows 환경 한정
        Debug.Log(UnityEngine.Windows.Directory.temporaryFolder);
        Debug.Log(UnityEngine.Windows.Directory.roamingFolder);
#endif

        string path = Application.dataPath + "/Datatables";

        // 데이터를 읽어올 때 System.IO 네임스페이스가 여러 플랫폼을 지원
        string allText = File.ReadAllText($"{path}/SampleCSV.csv");
        string[] lines = allText.Split('\n');

        weaponsData = new(lines.Length << 1);
        foreach (string line in lines)
        {
            string[] cell = line.Split(',');

            if (false == int.TryParse(cell[0], out int index))
                continue;

            weaponsData.Add(index, new WeaponData()
            {
                index = index,
                name = cell[1],
                atk = int.Parse(cell[2]),
                dfe = int.Parse(cell[3]),
                description = cell[4],
            });
        }
        inspectorView = weaponsData.Values.ToArray();
    }
}

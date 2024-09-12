using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSample : MonoBehaviour
{
    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int level;
    }

    [SerializeField] private SaveData currentData;
    [SerializeField] private SaveData loadedData;
    private string path;

    private void Awake()
    {
#if UNITY_EDITOR
        path = Application.dataPath + "/EditorSaves";
#else
        path = Application.persistentDataPath;
#endif
    }

    [ContextMenu("Call Save")]
    private void Save()
    {
        if (false == Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText($"{path}/saveSample.json", json);
    }

    [ContextMenu("Call Load")]
    private void Load()
    {
        string filePath = $"{path}/saveSample.json";
        if (false == Directory.Exists(filePath))
        {
            Debug.Log("불러올 파일이 없음");
            return;
        }

        string json = File.ReadAllText(filePath);
        loadedData = JsonUtility.FromJson<SaveData>(json);
    }
}

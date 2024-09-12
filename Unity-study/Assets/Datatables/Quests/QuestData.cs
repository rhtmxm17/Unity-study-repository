using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest Data")]
public class QuestData : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField, TextArea] private string description;
    [SerializeField] private int reward;
    [SerializeField] private Texture icon;
    [SerializeField] private Color color;
    [SerializeField] private GameObject prefab;

    public string Title { get => title; set => title = value; }
    public string Description { get => description; }
    public int Reward { get => reward; }
    public Texture Icon { get => icon; }
    public Color Color { get => color; }

    public GameObject ClonePrefab()
    {
        return Instantiate(prefab);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class SimpleTextUI : MonoBehaviour
{
    [SerializeField] string label;
    private Text text;

    private void Awake()
    {
        if (!TryGetComponent(out text))
            Debug.LogError("Text 컴포넌트를 찾을 수 없음");
    }

    //// 제네릭 메서드는 인스펙터에서 찾을 수 없는듯?
    //public void UpdateText<T>(T item)
    //{
    //    text.text = string.Format(label, item);
    //}

    public void UpdateText(string item) => text.text = string.Format(label, item);
    public void UpdateText(int item) => text.text = string.Format(label, item);
    public void UpdateText(float item) => text.text = string.Format(label, item);
}

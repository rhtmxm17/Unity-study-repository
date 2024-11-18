using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPersonalSetting : MonoBehaviour
{
    [SerializeField] PersonalColors colorTable;

    [SerializeField] Renderer personalColorRenderer;
    [SerializeField] int targetMaterial = 0;

    public void SetColor(int index)
    {
        if (index < 0 || colorTable.Count <= index)
        {
            Debug.Log("잘못된 색상 번호");
            return;
        }

        personalColorRenderer.materials[targetMaterial].color = colorTable.GetColor(index);
    }
}

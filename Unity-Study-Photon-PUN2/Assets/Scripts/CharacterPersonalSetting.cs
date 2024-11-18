using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPersonalSetting : MonoBehaviourPun
{
    [SerializeField] PersonalColors colorTable;

    [SerializeField] Renderer personalColorRenderer;
    [SerializeField] int targetMaterial = 0;

    public Color PersonalColor { get; private set; }

    private void Start()
    {
        // 이미 설정된 내용이 있을 경우 적용
        int colorIndex = photonView.Owner.GetPersonalColor();
        if (colorIndex >= 0)
        {
            SetColor(colorIndex);
        }
    }

    public void SetColor(int index)
    {
        if (index < 0 || colorTable.Count <= index)
        {
            Debug.Log("잘못된 색상 번호");
            return;
        }

        PersonalColor = colorTable.GetColor(index);
        personalColorRenderer.materials[targetMaterial].color = PersonalColor;
        if (TryGetComponent(out ProjectileShooter setColorTarget))
        {
            setColorTarget.SetColor(PersonalColor);
        }
    }
}

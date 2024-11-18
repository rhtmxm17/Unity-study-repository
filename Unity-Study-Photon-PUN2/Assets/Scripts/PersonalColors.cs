using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalColors : ScriptableObject
{
    [SerializeField] Color[] colors;
    
    public int Count => colors.Length;
    public Color GetColor(int index) => colors[index];
}

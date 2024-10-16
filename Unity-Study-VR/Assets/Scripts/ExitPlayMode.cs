using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlayMode : MonoBehaviour
{
    [ContextMenu("Do Exit")]
    public void DoExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

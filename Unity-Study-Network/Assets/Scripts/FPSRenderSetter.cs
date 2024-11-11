using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSRenderSetter : MonoBehaviour
{
    [ContextMenu("Set Shadows Only")]
    public void SetShadowsOnly()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTest : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        LoadingManager.Instance.LoadSceneWithLoading(sceneName);
    }
}

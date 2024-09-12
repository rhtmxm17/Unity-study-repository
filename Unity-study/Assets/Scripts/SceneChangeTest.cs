using UnityEngine;

public class SceneChangeTest : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        LoadingManager.Instance.LoadSceneWithLoading(sceneName);
    }
}

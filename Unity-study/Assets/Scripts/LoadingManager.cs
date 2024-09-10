using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public static LoadingManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning("LoadingManager가 강제로 생성됨");
                GameObject emptyLoadingManager = new GameObject("Generated Loading Manager");
                instance = emptyLoadingManager.AddComponent<LoadingManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool AllowSceneActivation
    {
        get => allowSceneActivation;
        set
        {
            allowSceneActivation = value;
            if (loadingOperation != null)
                loadingOperation.allowSceneActivation = allowSceneActivation;
        }
    }

    public float Progress
    {
        get
        {
            if (loadingOperation == null)
                return -1f;
            else
                return loadingOperation.progress;
        }
    }

    public string nextSceneName;

    private AsyncOperation loadingOperation;
    private bool allowSceneActivation;

    public void StartLoadScene(string sceneName)
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = AllowSceneActivation;
        Debug.Log($"로딩 시작: {sceneName}");
        loadingOperation.completed += unused =>
        {
            Debug.Log($"씬 전환: {sceneName}");
            loadingOperation = null;
            nextSceneName = string.Empty;
        };
    }

    public void LoadSceneWithLoading(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
        //// 아직 활성화되지 않은 오브젝트라서 작동을 안함
        //var model = GameObject.FindWithTag("GameController").GetComponent<LoadingModel>();
        //model.nextSceneName = sceneName;
    }
}

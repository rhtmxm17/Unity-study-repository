using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning("GameManager가 강제로 생성됨");
                GameObject emptyGameManager = new GameObject("GameManager");
                instance = emptyGameManager.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    public enum GameState { None, Ready, Running, GameOver };

    public UnityEvent<GameState> OnGameStateChanged;

    [SerializeField] private GameState state;

    public GameState State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            state = value;
            OnGameStateChanged?.Invoke(state);
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
        State = GameState.Ready;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Ready:
                {
                    if (Input.anyKeyDown)
                        State = GameState.Running;
                }
                break;
            case GameState.Running:
                break;
            case GameState.GameOver:
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        SceneManager.LoadScene("DodgeGameScene");
                        State = GameState.Ready;
                    }
                }
                break;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글턴
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

    // 게임 상태
    public enum GameState { None, Ready, Running, GameOver, Win };

    public event UnityAction<GameState> OnGameStateChanged;

    [SerializeField] private GameState state;

    public GameState State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            if (value == GameState.Running) // 스테이지 시작일 경우 시작시간 기록
            {
                StageStartTime = Time.time;
            }
            else if (state == GameState.Running) // 스테이지 종료일 경우 스코어 결정
            {
                currentScore = Time.time - StageStartTime;

                if (value == GameState.Win && BestScore < currentScore) // 기록 갱신
                    BestScore = currentScore;
            }

            state = value;
            OnGameStateChanged?.Invoke(state);
        }
    }

    // 스코어 관련
    public float StageStartTime { get; private set; }

    private float currentScore;
    public float CurrentScore
    {
        get
        {
            if (State == GameState.Running)
                return Time.time - StageStartTime;
            else
                return currentScore;
        }
    }

    public float BestScore { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
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
                    {
                        State = GameState.Running;
                    }
                }
                break;
            case GameState.Running:
                break;
            case GameState.GameOver:
            case GameState.Win:
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

    private void OnDisable()
    {
        if (OnGameStateChanged == null)
            Debug.Log("delegate가 정상적으로 비어있음");
        else
            Debug.Log($"delegate가 남아있음: {OnGameStateChanged.GetInvocationList()}");
    }

    // 게임오브젝트를 비활성화시 코루틴도 정지하는듯...
    // 매니저에서 대리로 코루틴을 돌려줌
    public void EntrustCoroutine(IEnumerator routine) => StartCoroutine(routine);

}

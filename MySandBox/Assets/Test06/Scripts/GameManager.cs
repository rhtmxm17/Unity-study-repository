using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Test06
{
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
        public enum GameState { Ready, Running, GameOver };

        public event UnityAction<GameState> OnGameStateChanged;

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
            // 싱글턴
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            //초기화
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
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            SceneManager.LoadScene("Test06");
                            State = GameState.Ready;
                        }
                    }
                    break;
            }
        }
    }
}
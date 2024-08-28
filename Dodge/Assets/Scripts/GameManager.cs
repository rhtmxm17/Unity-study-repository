using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { None, Ready, Running, GameOver };

    [SerializeField] private GameState state;
    [SerializeField] private Text uiReady;
    [SerializeField] private Text uiGameOver;

    public GameState State
    {
        get { return state; }
        set
        {
            if (state == value)
                return;

            state = value;
            OnGameStateChanged?.Invoke(state);

            uiReady.gameObject.SetActive(state == GameState.Ready);
            uiGameOver.gameObject.SetActive(state == GameState.GameOver);
        }
    }

    public event UnityAction<GameState> OnGameStateChanged;

    private void Awake()
    {
        State = GameState.Ready;
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
                    }
                }
                break;
        }
    }
}

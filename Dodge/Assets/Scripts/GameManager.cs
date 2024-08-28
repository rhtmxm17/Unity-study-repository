using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum GameState { Ready, Running, GameOver };

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

    public event UnityAction<GameState> OnGameStateChanged;

    private void Awake()
    {
        state = GameState.Ready;
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
                break;
        }
    }

    public void GameStart()
    {

    }

    public void GameOver()
    {

    }
}

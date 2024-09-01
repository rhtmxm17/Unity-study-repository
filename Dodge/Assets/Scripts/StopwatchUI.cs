using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchUI : MonoBehaviour
{
    [SerializeField] private float stopwatchTick = 1f / 60f;

    private Text text;
    private Coroutine updateStopwatch;
    private WaitForSeconds waitUpdateStopwatchTick;

    private void Awake()
    {
        waitUpdateStopwatchTick = new(stopwatchTick);
        if (!TryGetComponent(out text))
            Debug.LogError("Text 컴포넌트를 찾을 수 없음");
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += SwitchUI;
        SwitchUI(GameManager.Instance.State);
    }

    private void SwitchUI(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Running:
                updateStopwatch = StartCoroutine(Stopwatch());
                break;
            case GameManager.GameState.GameOver:
                if (updateStopwatch != null)
                {
                    StopCoroutine(updateStopwatch);
                    updateStopwatch = null;
                }
                break;
            default:
                if (updateStopwatch != null)
                {
                    StopCoroutine(updateStopwatch);
                    updateStopwatch = null;
                }
                text.text = string.Empty;
                break;
        }
    }

    private IEnumerator Stopwatch()
    {
        while (true)
        {
            text.text = $"{Time.time - GameManager.Instance.StageStartTime:0.00}s";
            yield return waitUpdateStopwatchTick;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= SwitchUI;
    }
}

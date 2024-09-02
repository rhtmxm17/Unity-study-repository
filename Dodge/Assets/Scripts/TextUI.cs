using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
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
            case GameManager.GameState.Ready:
                text.text = "아무 키나 눌러서 시작";
                break;
            case GameManager.GameState.GameOver:
                text.text = "게임 오버\n스페이스바를 눌러서 다시 시작";
                break;
            case GameManager.GameState.Win:
                text.text = $"클리어!\n현재 점수: {GameManager.Instance.CurrentScore}\n최고 점수: {GameManager.Instance.BestScore}\n스페이스바를 눌러서 다시 시작";
                break;
            default:
                text.text = string.Empty;
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= SwitchUI;
    }
}

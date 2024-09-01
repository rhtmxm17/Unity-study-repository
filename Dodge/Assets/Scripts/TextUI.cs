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
                text.text = "Press Any Key";
                break;
            case GameManager.GameState.GameOver:
                text.text = "Game Over\nPress Space for Restart";
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

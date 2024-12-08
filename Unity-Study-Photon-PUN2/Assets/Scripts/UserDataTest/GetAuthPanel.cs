using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetAuthPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(Auth);
    }

    private void OnEnable()
    {
        loginButton.interactable = true;
    }

    private void Auth()
    {
        if (idInputField.text == string.Empty)
        {
            Debug.Log("플레이어 이름이 비어있습니다");
            return;
        }

        loginButton.interactable = false;

        BackendManager.Auth.SignInWithEmailAndPasswordAsync(idInputField.text, passwordInputField.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소됨.");
                loginButton.interactable = true;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패: " + task.Exception);
                loginButton.interactable = true;
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"로그인 성공: {result.User.DisplayName}");
            LogIn();
        });

    }

    private void LogIn()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
        {
            Debug.Log("잘못된 접근입니다");
            return;
        }

        this.gameObject.SetActive(false);
    }
}

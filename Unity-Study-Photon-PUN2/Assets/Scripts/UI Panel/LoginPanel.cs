using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] Button loginButton;
    [SerializeField] Button signUpButton;
    [SerializeField] GameObject signUpPanel;

    private void Start()
    {
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
        loginButton.onClick.AddListener(Auth);
        signUpButton.onClick.AddListener(() => signUpPanel.SetActive(true));
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
            Debug.Log($"로그인 성공: {result.User.DisplayName} ({result.User.UserId})");

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

        //if (false == user.IsEmailVerified)
        //{
        //    // TODO: 이메일 인증
        //    return;
        //}

        if (user.DisplayName == string.Empty)
        {
            // TODO: 닉네임 설정
            // user.UpdateUserProfileAsync()
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
        PhotonNetwork.ConnectUsingSettings();
    }
}

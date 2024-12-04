using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections;
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
    [SerializeField] SignUpPanel signUpPanel;
    [SerializeField] RectTransform emailVerificationPanel;

    private void Start()
    {
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
        loginButton.onClick.AddListener(Auth);
        signUpButton.onClick.AddListener(() => signUpPanel.gameObject.SetActive(true));

        signUpPanel.onSendEmailVerification.AddListener(() => StartCoroutine(WaitEmailVerification()));
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

        if (false == user.IsEmailVerified)
        {
            // TODO: 이메일 인증
            Debug.Log("이메일 인증 필요");
            StartCoroutine(WaitEmailVerification());
            return;
        }

        if (user.DisplayName == string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = "플레이어";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    private IEnumerator WaitEmailVerification()
    {
        emailVerificationPanel.gameObject.SetActive(true);
        YieldInstruction checkPeriod = new WaitForSeconds(1f);
        bool reloadCompleted = true;

        while (true)
        {
            // 아직 지난번 주기의 ReloadAsync가 완료되지 않았다면 건너뛰기
            if (reloadCompleted)
            {
                reloadCompleted = false;
                BackendManager.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
                {
                    reloadCompleted = true;
                    if (task.IsCanceled)
                    {
                        Debug.LogError("인증 정보 확인 취소됨.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("인증 정보 확인 실패: " + task.Exception);
                        return;
                    }

                    if (BackendManager.Auth.CurrentUser.IsEmailVerified)
                    {
                        Debug.Log("이메일 인증 확인");
                        emailVerificationPanel.gameObject.SetActive(false);
                        LogIn();
                        return;
                    }
                });

            }
            yield return checkPeriod;
        }
    }
}

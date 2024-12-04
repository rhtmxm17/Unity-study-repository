using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] Button signUpButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button emailConfirmButton;
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField confirmInputField;

    public UnityEvent onSendEmailVerification;

    private void Start()
    {
        signUpButton.onClick.AddListener(SignUp);
        emailConfirmButton.onClick.AddListener(ConrirmEmail);
        cancelButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        emailInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        passwordInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        confirmInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        CheckEmptyInputField();
        signUpButton.interactable = false;
    }

    private void ConrirmEmail()
    {
        // 이메일 중복 검사가 권장되지 않아 Firebase Auth에서 기본값이 비활성화됨
        Debug.Log("이메일 중복 검사가 권장되지 않아 Firebase Auth에서 기본값이 비활성화됨");
        signUpButton.interactable = true;
    }

    private void SignUp()
    {

        if (passwordInputField.text != confirmInputField.text)
        {
            Debug.Log("비밀번호를 다시 확인해주세요");
            return;
        }

        // Firebase를 통해 계정 생성이 진행되는 동안 입력 비활성화
        signUpButton.interactable = false;
        emailInputField.interactable = false;
        passwordInputField.interactable = false;
        confirmInputField.interactable = false;


        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("계정 생성 취소됨.");
                ResetInputField();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("계정 생성 실패: " + task.Exception);
                ResetInputField();
                return;
            }

            Debug.Log($"가계정이 성공적으로 생성됨, 인증 메일 발송을 시작합니다.");
            // Firebase user has been created.
            AuthResult result = task.Result;
            result.User.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("인증 메일 전송 취소됨.");
                    ResetInputField();
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("계정 생성 실패: " + task.Exception);
                    ResetInputField();
                    return;
                }
                Debug.Log($"인증 메일 발송 완료.");
                onSendEmailVerification?.Invoke();
                this.gameObject.SetActive(false);
            });
        });
    }

    private void ResetInputField()
    {
        emailInputField.interactable = true;
        passwordInputField.interactable = true;
        confirmInputField.interactable = true;
        CheckEmptyInputField();
    }

    private void CheckEmptyInputField()
    {
        signUpButton.interactable = 
            (emailInputField.text != string.Empty
            && passwordInputField.text != string.Empty
            && confirmInputField.text != string.Empty);
    }
}

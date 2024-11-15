using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] Button signUpButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField confirmInputField;

    private void Start()
    {
        signUpButton.onClick.AddListener(SignUp);
        cancelButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        emailInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        passwordInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        confirmInputField.onEndEdit.AddListener(_ => CheckEmptyInputField());
        CheckEmptyInputField();
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
                emailInputField.interactable = true;
                passwordInputField.interactable = true;
                confirmInputField.interactable = true;
                CheckEmptyInputField();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("계정 생성 실패: " + task.Exception);
                emailInputField.interactable = true;
                passwordInputField.interactable = true;
                confirmInputField.interactable = true;
                CheckEmptyInputField();
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"계정이 성공적으로 생성됨: {result.User.DisplayName} ({result.User.UserId})");
        });
    }

    private void CheckEmptyInputField()
    {
        signUpButton.interactable = 
            (emailInputField.text != string.Empty
            && passwordInputField.text != string.Empty
            && confirmInputField.text != string.Empty);
    }
}

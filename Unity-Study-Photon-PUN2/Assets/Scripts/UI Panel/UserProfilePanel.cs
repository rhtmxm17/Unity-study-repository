using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserProfilePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField nickNameInputField;
    [SerializeField] Button nickNameConfirmButton;

    private void Start()
    {
        nickNameConfirmButton.onClick.AddListener(UpdateNickName);
    }

    private void OnEnable()
    {
        if (BackendManager.Auth.CurrentUser == null)
        {
            Debug.Log("인증 정보가 존재하지 않습니다");
            this.enabled = false;
        }
    }

    private void UpdateNickName()
    {
        UserProfile profille = new UserProfile();

        profille.DisplayName = nickNameInputField.text;

        BackendManager.Auth.CurrentUser.UpdateUserProfileAsync(new UserProfile() { DisplayName = nickNameInputField.text }).ContinueWithOnMainThread(task =>
        {

        });
    }
}

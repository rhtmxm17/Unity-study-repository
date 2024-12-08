using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrnetUserDataPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] TMP_InputField jobInput;
    [SerializeField] Button submitButton;
    [SerializeField] Button levelupTestButton;
    [SerializeField] TMP_Text userInfoText;

    UserData userData = new UserData();

    private IEnumerator Start()
    {
        submitButton.onClick.AddListener(ChangeUserData);
        levelupTestButton.onClick.AddListener(LevelUp);
        yield return new WaitWhile(() => BackendManager.Auth == null);
        BackendManager.Auth.IdTokenChanged += GetUserData; // 인증 정보 갱신시 유저 정보 갱신
    }

    private void LevelUp()
    {
        userData.LevelUp();
        userData.SetCurrentUserFromDB(null);
        UpdateUserDataText(true);
    }

    private void GetUserData(object sender, System.EventArgs args)
    {
        userData.GetCurrentUserFromDB(UpdateUserDataText);
    }

    private void UpdateUserDataText(bool result)
    {
        if (false == result)
        {
            userInfoText.text = "아직 플레이어 정보가 생성되지 않았습니다.";
            return;
        }
        userInfoText.text = $"이름: {userData.UserName}\n직업: {userData.Job}\n레벨: {userData.Level}\nSTR: {userData.status.Str}\nDEX: {userData.status.Dex}\nINT: {userData.status.Int}\nLUK: {userData.status.Luk}";
    }

    private void ChangeUserData()
    {
        if (userData.UserName == null || userData.UserName == string.Empty)
        {
            CreateUserData();
            return;
        }

        userData.UserName = userNameInput.text;
        userData.Job = jobInput.text;
        userData.SetCurrentUserFromDB(null);
        UpdateUserDataText(true);
    }

    private void CreateUserData()
    {
        userData = new UserData()
        {
            UserName = userNameInput.text,
            Job = jobInput.text,
            Level = 1,
            status = new Status()
            {
                Str = 10,
                Dex = 10,
                Int = 10,
                Luk = 10
            }
        };

        BackendManager.CurrentUserDataRef.SetRawJsonValueAsync(JsonUtility.ToJson(userData)).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log($"유저 데이터 생성 실패: {task.Exception}");
                return;
            }

            Debug.Log($"유저 데이터 생성됨");
            UpdateUserDataText(true);
        });
    }


}

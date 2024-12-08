using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchUserDataPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] TMP_InputField userNameInput;
    [SerializeField] Button submitButton;
    [SerializeField] TMP_Text userInfoText;

    private UserData userData = new UserData();

    private void Start()
    {
        submitButton.onClick.AddListener(Submit);
    }

    private void Submit()
    {
        userData.GetDataFromDB(userNameInput.text, UpdateUserData);
    }

    private void UpdateUserData(bool result)
    {
        if (false == result)
        {
            userInfoText.text = "존재하지 않는 플레이어 입니다.";
            return;
        }

        userInfoText.text = $"이름: {userData.UserName}\n직업: {userData.Job}\n레벨: {userData.Level}\nSTR: {userData.status.Str}\nDEX: {userData.status.Dex}\nINT: {userData.status.Int}\nLUK: {userData.status.Luk}";
    }
}

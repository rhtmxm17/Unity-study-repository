using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public const int RoomPlayersMax = 8;

    [Header("Child UI")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button randomMatchingButton;
    [SerializeField] Button lobbyButton;
    [SerializeField] Button logoutButton;
    [SerializeField] Button editInfoButton;

    [Header("Create Room")]
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField maxPlayerInputField;
    [SerializeField] Button createRoomConfirmButton;
    [SerializeField] Button createRoomCancelButton;

    [Header("Edit Info")]
    [SerializeField] GameObject editInfoPanel;
    [SerializeField] TMP_InputField passwordResubmitField;
    [SerializeField] Button passwordResubmitButton;
    [SerializeField] TMP_InputField newPasswordField;
    [SerializeField] Button newPasswordButton;
    [SerializeField] TMP_InputField newNicknameField;
    [SerializeField] Button newNicknameButton;
    [SerializeField] Button deleteAccountButton;
    [SerializeField] Button editInfoCancelButton;

    [Header("Delete Account")]
    [SerializeField] GameObject deleteAccountPanel;
    [SerializeField] Button deleteAccountSubmitButton;
    [SerializeField] TMP_Text deleteAccountSubmitButtonText;
    [SerializeField] Button deleteAccountCancelButton;

    private void Start()
    {
        createRoomButton.onClick.AddListener(CreateRoomMenu);
        randomMatchingButton.onClick.AddListener(RandomMatching);
        lobbyButton.onClick.AddListener(JoinLobby);
        logoutButton.onClick.AddListener(Logout);
        editInfoButton.onClick.AddListener(OpenEditInfoMenu);

        createRoomConfirmButton.onClick.AddListener(CreateRoomConfirm);
        createRoomCancelButton.onClick.AddListener(CreateRoomCancel);

        passwordResubmitButton.onClick.AddListener(ResubmitPassword);
        newPasswordButton.onClick.AddListener(EditPassword);
        newNicknameButton.onClick.AddListener(EditNickName);
        deleteAccountButton.onClick.AddListener(OpenDeleteAccountMenu);
        editInfoCancelButton.onClick.AddListener(EditInfoCancel);

        deleteAccountSubmitButton.onClick.AddListener(DeleteAccount);
        deleteAccountCancelButton.onClick.AddListener(DeleteAccountCancel);

    }

    private void OnEnable()
    {
        menuPanel.SetActive(true);
        createRoomPanel.SetActive(false);

        roomNameInputField.text = $"Room {Random.Range(1000, 10000)}";
        maxPlayerInputField.text = "8";
    }

    public void CreateRoomMenu()
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomConfirm()
    {
        if (roomNameInputField.text == string.Empty)
        {
            Debug.LogWarning("방 이름이 비어있습니다");
            return;
        }

        int maxPlayers;
        if (false == int.TryParse(maxPlayerInputField.text, out maxPlayers))
        {
            Debug.LogWarning("플레이어 수가 비어있거나 잘못되었습니다");
            return;
        }
        maxPlayers = Mathf.Clamp(maxPlayers, 1, RoomPlayersMax);

        Debug.Log($"방 생성");
        RoomOptions options = new RoomOptions()
        {
            MaxPlayers = maxPlayers
        };
        PhotonNetwork.CreateRoom(roomNameInputField.text, options);
    }

    public void CreateRoomCancel()
    {
        createRoomPanel.SetActive(false);
    }

    public void RandomMatching()
    {
        Debug.Log("랜덤 매칭 시도");
        //PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void JoinLobby()
    {
        Debug.Log("로비 진입 시도");
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        Debug.Log("접속을 종료합니다");
        PhotonNetwork.Disconnect();
    }

    private void OpenEditInfoMenu()
    {
        editInfoPanel.SetActive(true);
        passwordResubmitField.interactable = true;
        passwordResubmitButton.interactable = true;

        newPasswordField.interactable = false;
        newPasswordButton.interactable = false;
        newNicknameField.interactable = false;
        newNicknameButton.interactable = false;

        passwordResubmitField.text = newPasswordField.text = newNicknameField.text = string.Empty;
    }

    public void EditInfoCancel()
    {
        editInfoPanel.SetActive(false);
    }

    private void ResubmitPassword()
    {
        passwordResubmitField.interactable = false;
        passwordResubmitButton.interactable = false;
        Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(BackendManager.Auth.CurrentUser.Email, passwordResubmitField.text);
        BackendManager.Auth.CurrentUser.ReauthenticateAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("재인증 실패: " + task.Exception);
                passwordResubmitField.interactable = true;
                passwordResubmitButton.interactable = true;
                return;
            }

            Debug.Log("재인증 성공");

            newPasswordField.interactable = true;
            newPasswordButton.interactable = true;
            newNicknameField.interactable = true;
            newNicknameButton.interactable = true;
        });
    }

    private void EditPassword()
    {
        if (newPasswordField.text == string.Empty)
        {
            Debug.Log("패스워드가 비어있음");
            return;
        }

        newPasswordField.interactable = false;
        newPasswordButton.interactable = false;
        BackendManager.Auth.CurrentUser.UpdatePasswordAsync(newPasswordField.text).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("패스워드 변경 실패: " + task.Exception);
            }
            else
            {
                Debug.Log("패스워드 변경 성공");
            }

            newPasswordField.interactable = true;
            newPasswordButton.interactable = true;
        });
    }

    private void EditNickName()
    {
        if (newNicknameField.text == string.Empty)
        {
            Debug.Log("닉네임이 비어있음");
            return;
        }

        newNicknameField.interactable = false;
        newNicknameButton.interactable = false;
        BackendManager.Auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile() { DisplayName = newNicknameField.text }).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("닉네임 변경 실패: " + task.Exception);
            }
            else
            {
                Debug.Log("닉네임 변경 성공");
            }

            PhotonNetwork.LocalPlayer.NickName = newNicknameField.text;
            newNicknameField.interactable = true;
            newNicknameButton.interactable = true;
        });
    }

    private void OpenDeleteAccountMenu()
    {
        deleteAccountPanel.SetActive(true);
        deleteAccountSubmitButton.interactable = false;
        StartCoroutine(DeleteSubmitButtonCountdown());
    }

    private IEnumerator DeleteSubmitButtonCountdown()
    {
        YieldInstruction wait1Sec = new WaitForSeconds(1f);
        int countdown = 3;
        while (countdown > 0)
        {
            deleteAccountSubmitButtonText.text = $"예({countdown})";
            yield return wait1Sec;

            countdown--;
        }

        deleteAccountSubmitButtonText.text = $"예";
        deleteAccountSubmitButton.interactable = true;
    }

    private void DeleteAccountCancel()
    {
        deleteAccountPanel.SetActive(false);
    }

    private void DeleteAccount()
    {
        BackendManager.Auth.CurrentUser.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogWarning("계정 삭제 실패: " + task.Exception);
                return;
            }

            Debug.Log("계정 삭제 성공");
            PhotonNetwork.Disconnect();
        });
    }
}

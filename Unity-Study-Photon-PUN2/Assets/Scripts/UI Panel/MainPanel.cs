using Photon.Pun;
using Photon.Realtime;
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
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField maxPlayerInputField;
    [SerializeField] Button createRoomConfirmButton;
    [SerializeField] Button createRoomCancelButton;

    private void Start()
    {
        createRoomButton.onClick.AddListener(CreateRoomMenu);
        randomMatchingButton.onClick.AddListener(RandomMatching);
        lobbyButton.onClick.AddListener(JoinLobby);
        logoutButton.onClick.AddListener(Logout);
        createRoomConfirmButton.onClick.AddListener(CreateRoomConfirm);
        createRoomCancelButton.onClick.AddListener(CreateRoomCancel);
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
}

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] PlayerEntry[] playerEntries;
    [SerializeField] Button startButton;
    [SerializeField] Button leaveButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        leaveButton.onClick.AddListener(LeaveRoom);
        UpdatePlayerEntries();
    }

    private void OnEnable()
    {
        if (false == PhotonNetwork.InRoom)
        {
            Debug.LogWarning("잘못된 접근입니다");
            return;
        }

        // 방 로비 진입시 레디 상태 해제(방 재진입이나 게임 씬 종료시 필요)
        PhotonNetwork.LocalPlayer.SetReady(false);

        // 방장의 씬과 동기화
        PhotonNetwork.AutomaticallySyncScene = true;

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayerEntries;
        PlayerNumbering.OnPlayerNumberingChanged += () => { Debug.Log("OnPlayerNumberingChanged"); };
    }

    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayerEntries;
    }

    private void UpdatePlayerEntries()
    {
        foreach (PlayerEntry entry in playerEntries)
        {
            entry.SetEmpty();
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (0 > player.GetPlayerNumber())
                continue;

            playerEntries[player.GetPlayerNumber()].SetPlayerInfo(player);
        }

        startButton.interactable = GameStartCondition();
    }

    private bool GameStartCondition()
    {
        // 방장인지 검사
        if (false == PhotonNetwork.LocalPlayer.IsMasterClient)
            return false;

        // 모든 플레이어가 준비되었는지 검사
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (false == player.GetReady())
                return false;
        }

        // 모든 조건 통과시
        return true;
    }

    public void OnEnterPlayer(Player player)
    {
        Debug.Log($"{player.NickName} 입장");
        UpdatePlayerEntries();
    }

    public void OnExitPlayer(Player player)
    {
        Debug.Log($"{player.NickName} 퇴장");
        UpdatePlayerEntries();
    }

    public void OnPlayerPropertyChanged(Player targetPlayer, PhotonHashtable changedProps)
    {
        Debug.Log($"RoomPanel: OnPlayerPropertyChanged");
        UpdatePlayerEntries();
    }

    public void StartGame()
    {
        if (false == PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("잘못된 접근입니다");
            return;
        }

        PhotonNetwork.LoadLevel("LevelScene");

        // 게임 진행 도중 참여가 불가능한 게임이라면
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void LeaveRoom()
    {
        Debug.Log("방에서 나갑니다");
        PhotonNetwork.LeaveRoom();
    }
}

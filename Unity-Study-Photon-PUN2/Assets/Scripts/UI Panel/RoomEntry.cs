using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    private void Start()
    {
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    public void SetRoomInfoText(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name;
        currentPlayer.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        joinRoomButton.interactable = roomInfo.IsOpen;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }
}

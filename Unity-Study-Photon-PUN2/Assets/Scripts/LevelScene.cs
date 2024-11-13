using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class LevelScene : MonoBehaviourPunCallbacks
{
    [SerializeField] Button gameOverButton;
    [SerializeField] Button returnLobbyButton;

    private void Start()
    {
        gameOverButton.onClick.AddListener(GameOver);
        returnLobbyButton.onClick.AddListener(ReturnLobby);
        UpdateMasterClient();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateMasterClient();
    }

    private void UpdateMasterClient()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameOverButton.interactable = true;
        }
        else
        {
            gameOverButton.interactable = false;
        }
    }

    public void GameOver()
    {
        if (false == PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("LobbyScene");
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    public void ReturnLobby()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}

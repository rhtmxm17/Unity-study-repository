using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneTester : MonoBehaviourPunCallbacks
{
    public const string TestRoomName = "TestRoom";

    private LevelScene levelScene;

    private void Awake()
    {
        // 로비를 통해 들어와서 이미 연결되어 있을 경우 사용하지 않는다
        if (PhotonNetwork.IsConnected)
        {
            Destroy(this);
        }
        
        levelScene = GetComponent<LevelScene>();
        PhotonTransformView transformView = GetComponent<PhotonTransformView>();
    }

    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(TestRoomName,
            new RoomOptions()
            {
                IsVisible = false,
                MaxPlayers = 8,
            },
            TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("테스트룸 생성");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("테스트룸 참가");

        levelScene.OnJoinScene();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 진입");
    }

}

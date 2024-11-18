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

        if (PhotonNetwork.InRoom)
        {
            OnJoinScene();
        }
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (DictionaryEntry pair in changedProps)
        {
            switch ((string)pair.Key)
            {
                case CustomPropertyExtension.CharacterVidKey:
                    UpdatePersonalSettings(targetPlayer, (int)pair.Value);
                    break;
                case CustomPropertyExtension.PersonalColorIndexKey:
                    UpdatePersonalColor(targetPlayer, (int)pair.Value);
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdatePersonalSettings(Player targetPlayer, int viewId)
    {
        PhotonView playerCharacter = PhotonNetwork.GetPhotonView(viewId);
        if (playerCharacter == null)
            return;

        if (false == playerCharacter.TryGetComponent(out CharacterPersonalSetting setting))
            return;

        int colorIndex = targetPlayer.GetPersonalColor();
        if (colorIndex >= 0)
        {
            setting.SetColor(colorIndex);
        }
    }

    private void UpdatePersonalColor(Player targetPlayer, int colorIndex)
    {
        int characterVid = targetPlayer.GetCharacterVid();
        if (characterVid > 0) // 기술문서에 의하면 view ID 0은 '정의되지 않음'을 의미
        {
            PhotonView playerCharacter = PhotonNetwork.GetPhotonView(characterVid);
            if (playerCharacter == null)
                return;

            if (false == playerCharacter.TryGetComponent(out CharacterPersonalSetting setting))
                return;

            setting.SetColor(colorIndex);
        }
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
        PhotonNetwork.LeaveRoom();

        //// 방으로 잠시 돌아갔다 다시 진행중인 게임 참여는 조금 더 복잡한 처리가 필요한듯
        //PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void OnJoinScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ReadySceneMaster();
        }

        ReadyPlayer();
    }

    private void ReadySceneMaster()
    {
        //// 포톤뷰 객체를 RoomObject로 생성
        //PhotonNetwork.InstantiateRoomObject("Monster", new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)), Quaternion.identity);
    }

    private void ReadyPlayer()
    {
        PhotonNetwork.Instantiate("PlayerCharacter", new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f)), Quaternion.identity);
    }

}

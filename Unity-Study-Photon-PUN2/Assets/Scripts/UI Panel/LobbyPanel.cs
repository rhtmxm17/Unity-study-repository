using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RoomEntry roomEntryPrefab;
    [Header("Child UI")]
    [SerializeField] RectTransform roomContent;
    [SerializeField] Button leaveButton;

    private Dictionary<string, RoomEntry> roomDictionary = new Dictionary<string, RoomEntry>();

    private void Start()
    {
        leaveButton.onClick.AddListener(LeaveLobby);
    }

    private void OnDisable()
    {
        ClearRoomDictionary();
    }

    public void LeaveLobby()
    {
        Debug.Log("로비를 나갑니다");
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            // 기존의 방이 사라진 경우
            if (roomInfo.RemovedFromList)
            {
                // 로비 진입시점에 사라진 방의 정보를 받은 케이스
                if (false == roomDictionary.ContainsKey(roomInfo.Name))
                    continue;

                Destroy(roomDictionary[roomInfo.Name].gameObject);
                roomDictionary.Remove(roomInfo.Name);

            }

            // 새로운 방이 추가된 경우
            else if (false == roomDictionary.ContainsKey(roomInfo.Name))
            {
                RoomEntry instance = Instantiate(roomEntryPrefab, roomContent);
                roomDictionary.Add(roomInfo.Name, instance);
                instance.SetRoomInfoText(roomInfo);
            }

            // 기존 방의 정보가 갱신된 경우
            else
            {
                roomDictionary[roomInfo.Name].SetRoomInfoText(roomInfo);
            }
        }
    }

    private void ClearRoomDictionary()
    {
        foreach(var pair in roomDictionary)
        {
            Destroy(pair.Value.gameObject);
        }
        roomDictionary.Clear();
    }
}

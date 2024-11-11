using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] NetworkObject playerPrefab;
    [SerializeField] GameObject playerUI;

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"플레이어{player.PlayerId} 접속");

        // 해당 플레이어의 클라이언트에서만 생성을 호출하도록 제한한다
        // 이러한 예외 처리가 누락될 경우,
        // 아래 내용이 모든 플레이어의 클라이언트에서 각각 실행되어
        // 신규 플레이어의 캐릭터가 참여중인 플레이어 수 만큼 생성된다
        if (player != Runner.LocalPlayer)
            return;

        // 네트워크 상에서 공유되는 물체는 Runner.Spawn()으로 생성
        // 무작위 위치에 생성해도 모든 클라이언트에서 동일한 위치에 나타난다
        NetworkObject newPlayer = Runner.Spawn(playerPrefab, new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)));

        // 해당 클라이언트에 한정된 물체는 기존의 Instantiate()로 생성
        if (playerUI != null)
            Instantiate(playerUI);
    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"플레이어{player.PlayerId} 접속 해제");

    }
}

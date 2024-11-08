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
        Debug.Log($"�÷��̾�{player.PlayerId} ����");

        // �ش� �÷��̾��� Ŭ���̾�Ʈ������ ������ ȣ���ϵ��� �����Ѵ�
        // �̷��� ���� ó���� ������ ���,
        // �Ʒ� ������ ��� �÷��̾��� Ŭ���̾�Ʈ���� ���� ����Ǿ�
        // �ű� �÷��̾��� ĳ���Ͱ� �������� �÷��̾� �� ��ŭ �����ȴ�
        if (player != Runner.LocalPlayer)
            return;
        
        // ��Ʈ��ũ �󿡼� �����Ǵ� ��ü�� Runner.Spawn()���� ����
        // ������ ��ġ�� �����ص� ��� Ŭ���̾�Ʈ���� ������ ��ġ�� ��Ÿ����
        Runner.Spawn(playerPrefab, new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f)));

        // �ش� Ŭ���̾�Ʈ�� ������ ��ü�� ������ Instantiate()�� ����
        if (playerUI != null )
            Instantiate(playerUI);

    }

    public void PlayerLeft(PlayerRef player)
    {
        Debug.Log($"�÷��̾�{player.PlayerId} ���� ����");

    }
}

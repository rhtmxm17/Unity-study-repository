using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLeftAnnounce : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] MainUI mainUI;

    public void PlayerJoined(PlayerRef player)
    {
        mainUI.Announce($"{player} 접속");
    }

    public void PlayerLeft(PlayerRef player)
    {
        mainUI.Announce($"{player} 접속 해제");
    }
}

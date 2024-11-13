using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 사용할 커스텀 프로퍼티와 그 형변환을 모아서 관리한다
/// </summary>
public static class CustomPropertyExtension
{
    private const string PlayerReady = "Rd";
    private const bool DefaultPlayerReady = false;
    private static PhotonHashtable playerReadyHashTable = new PhotonHashtable() { { PlayerReady, DefaultPlayerReady } };

    public static void SetReady(this Player player, bool value)
    {
        playerReadyHashTable[PlayerReady] = value;
        player.SetCustomProperties(playerReadyHashTable); // SetCustomProperties를 통해 변경해야 동기화 된다
    }

    public static bool GetReady(this Player player)
    {
        if (player.CustomProperties.TryGetValue(PlayerReady, out object value))
            return (bool)value;
        else
            return DefaultPlayerReady;
    }
}

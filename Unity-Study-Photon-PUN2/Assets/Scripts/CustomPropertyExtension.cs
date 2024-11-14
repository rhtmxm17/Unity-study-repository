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
    private class CustomPlayerProperty<T> where T : struct
    {
        public CustomPlayerProperty(string key, T defaultvValue)
        {
            this.Key = key;
            this.defaultValue = defaultvValue;
            this.table = new PhotonHashtable() { { key, defaultvValue } };
        }

        private string Key;
        private T defaultValue;
        private PhotonHashtable table;

        public void Set(Player player, T value)
        {
            table[Key] = value;
            player.SetCustomProperties(table);
        }

        public T Get(Player player)
        {
            if (player.CustomProperties.TryGetValue(Key, out object value))
                return (T)value;
            else
                return defaultValue;
        }
    }

    private static CustomPlayerProperty<bool> ready = new CustomPlayerProperty<bool>("Rd", false);
    private static CustomPlayerProperty<bool> load = new CustomPlayerProperty<bool>("ld", false);

    public static void SetReady(this Player player, bool value) => ready.Set(player, value);
    public static bool GetReady(this Player player) => ready.Get(player);

    public static void SetLoad(this Player player, bool value) => load.Set(player, value);
    public static bool GetLoad(this Player player) => load.Get(player);
}



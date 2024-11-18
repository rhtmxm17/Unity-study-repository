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
            this.key = key;
            this.defaultValue = defaultvValue;
            this.table = new PhotonHashtable() { { key, defaultvValue } };
        }

        private readonly string key;
        private readonly T defaultValue;
        private readonly PhotonHashtable table;

        public void Set(Player player, T value)
        {
            table[key] = value;
            player.SetCustomProperties(table);
        }

        public T Get(Player player)
        {
            if (player.CustomProperties.TryGetValue(key, out object value))
                return (T)value;
            else
                return defaultValue;
        }
    }

    public const string ReadyKey = "Rd";
    public const string LoadKey = "ld";
    public const string CharacterVidKey = "Cv";
    public const string PersonalColorIndexKey = "Pci";

    private static readonly CustomPlayerProperty<bool> ready = new CustomPlayerProperty<bool>(ReadyKey, false);
    private static readonly CustomPlayerProperty<bool> load = new CustomPlayerProperty<bool>(LoadKey, false);
    private static readonly CustomPlayerProperty<int> characterVid = new CustomPlayerProperty<int>(CharacterVidKey, 0);
    private static readonly CustomPlayerProperty<int> personalColorIndex = new CustomPlayerProperty<int>(PersonalColorIndexKey, -1);

    public static void SetReady(this Player player, bool value) => ready.Set(player, value);
    public static bool GetReady(this Player player) => ready.Get(player);

    public static void SetLoad(this Player player, bool value) => load.Set(player, value);
    public static bool GetLoad(this Player player) => load.Get(player);

    public static void SetCharacterVid(this Player player, int value) => characterVid.Set(player, value);
    public static int GetCharacterVid(this Player player) => characterVid.Get(player);

    public static void SetPersonalColor(this Player player, int value) => personalColorIndex.Set(player, value);
    public static int GetPersonalColor(this Player player) => personalColorIndex.Get(player);
}



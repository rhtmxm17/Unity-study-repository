using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Status
{
    public int Str;
    public int Dex;
    public int Int;
    public int Luk;
}

[System.Serializable]
public class UserData
{
    public string UserName;
    public string Job;
    public int Level;
    public Status status;

    /// <summary>
    /// 레벨업과 그에 따른 스텟 증가, 로컬에만 반영
    /// </summary>
    public void LevelUp()
    {
        this.Level++;

        // 플레이어 레벨업시 스텟 증가
        for (int i = 0; i < 5; i++)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    status.Str++;
                    break;
                case 1:
                    status.Dex++;
                    break;
                case 2:
                    status.Int++;
                    break;
                case 3:
                    status.Luk++;
                    break;
            }
        }
    }

    /// <summary>
    /// 인증된 유저의 데이터를 DB에서 읽어온다
    /// </summary>
    /// <param name="callback">읽어오기 완료시 성공 여부를 반환할 callback</param>
    /// <returns>읽어오기 요청 전송 성공여부</returns>
    public bool GetCurrentUserFromDB(UnityAction<bool> callback)
    {
        if (BackendManager.CurrentUserDataRef == null)
            return false;

        BackendManager.CurrentUserDataRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log($"작업이 취소되거나 실패함: {task.Exception}");
                return;
            }

            if (task.Result.Value == null)
            {
                // 아직 생성되지 않음
                callback?.Invoke(false);
                return;
            }

            JsonUtility.FromJsonOverwrite(task.Result.GetRawJsonValue(), this);
            callback?.Invoke(true);
        });

        return true;
    }

    /// <summary>
    /// 해당 이름을 갖는 유저의 데이터를 DB에서 읽어온다
    /// </summary>
    /// <param name="callback">읽어오기 완료시 성공 여부를 반환할 callback</param>
    /// <returns>읽어오기 요청 전송 성공여부</returns>
    public bool GetDataFromDB(string userName, UnityAction<bool> callback)
    {
        if (BackendManager.UserDataRef == null)
            return false;

        BackendManager.UserDataRef.OrderByChild("UserName").EqualTo(userName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log($"작업이 취소되거나 실패함: {task.Exception}");
                return;
            }

            if (task.Result.Value == null)
            {
                // 해당 유저명이 존재하지 않음
                callback?.Invoke(false);
                return;
            }

            JsonUtility.FromJsonOverwrite(task.Result.Children.First().GetRawJsonValue(), this);
            callback?.Invoke(true);
        });

        return true;
    }

    /// <summary>
    /// 데이터를 인증된 유저의 DB에 반영한다
    /// </summary>
    /// <param name="callback">쓰기 완료시 성공 여부를 반환할 callback</param>
    /// <returns>쓰기 요청 전송 성공여부</returns>
    public bool SetCurrentUserFromDB(UnityAction<bool> callback)
    {
        if (BackendManager.CurrentUserDataRef == null)
            return false;

        BackendManager.CurrentUserDataRef.SetRawJsonValueAsync(JsonUtility.ToJson(this)).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log($"작업이 취소되거나 실패함: {task.Exception}");
                return;
            }

            callback?.Invoke(true);
        });

        return true;
    }
}

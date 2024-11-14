using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterStatus : MonoBehaviour, IPunObservable
{
    [SerializeField] int level;
    [SerializeField] float hp;
    [SerializeField] bool alive;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 보낸 순서와 받는 순서가 일치해야 한다
        if (stream.IsWriting)
        {
            stream.SendNext(level);
            stream.SendNext(hp);
            stream.SendNext(alive);
        }

        if (stream.IsReading)
        {
            level = (int)stream.ReceiveNext();
            hp = (float)stream.ReceiveNext();
            alive = (bool)stream.ReceiveNext();
        }

        //stream.Serialize(ref level);
        //stream.Serialize(ref hp);
        //stream.Serialize(ref alive);
    }
}

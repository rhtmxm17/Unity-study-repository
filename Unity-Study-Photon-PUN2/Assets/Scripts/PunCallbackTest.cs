using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunCallbackTest : MonoBehaviourPunCallbacks
{
    [SerializeField] ClientState clientState;

    private void Update()
    {
        if (clientState == PhotonNetwork.NetworkClientState)
            return;

        clientState = PhotonNetwork.NetworkClientState;
        Debug.Log($"NetworkClientState: {clientState}");
    }
}

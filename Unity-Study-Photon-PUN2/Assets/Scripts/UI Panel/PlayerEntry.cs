using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text readyText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Button readyButton;
    [SerializeField] GameObject masterMarkImage;

    private void Start()
    {
        readyButton.onClick.AddListener(Ready);
    }

    private void OnEnable()
    {
        SetEmpty();
    }

    public void SetPlayerInfo(Player player = null)
    {
        if (player == null)
        {
            SetEmpty();
            return;
        }

        masterMarkImage.SetActive(player.IsMasterClient);
        nameText.text = player.NickName;
        readyButton.interactable = (PhotonNetwork.LocalPlayer == player);
        if (player.GetReady())
        {
            readyText.text = "Ready";
        }
        else
        {
            readyText.text = string.Empty;
        }

    }

    public void SetEmpty()
    {
        nameText.text = "<Empty>";
        readyButton.interactable = false;
        readyText.text = string.Empty;
        masterMarkImage.SetActive(false);
    }

    public void Ready()
    {
        Player local = PhotonNetwork.LocalPlayer;
        local.SetReady(! local.GetReady());

    }
}

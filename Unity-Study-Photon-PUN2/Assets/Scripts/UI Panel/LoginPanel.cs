using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [Header("Child UI")]
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] Button loginButton;

    private void Start()
    {
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
        loginButton.onClick.AddListener(Login);
    }

    private void OnEnable()
    {
        loginButton.interactable = true;
    }

    public void Login()
    {
        if (idInputField.text == string.Empty)
        {
            Debug.Log("플레이어 이름이 비어있습니다");
            return;
        }

        loginButton.interactable = false;
        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
        PhotonNetwork.ConnectUsingSettings();
    }
}

using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LoginControl _loginControl;

    private void OnValidate()
    {
        if (!_loginControl) Debug.LogWarning($"{name}:{nameof(UIManager)}.{nameof(_loginControl)} is not defined");
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _loginControl.SetActive(true);
    }
}
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private LoginControl _loginControl;
    [SerializeField] private EnterRoomControl _enterRoomControl;
    [SerializeField] private LobbyControl _lobbyControl;
    [SerializeField] private Volume _UIBackgroundVFX;

    private bool _firstLogin = true;

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

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("Scene01");
        _enterRoomControl.SetActive(false);
        _lobbyControl.SetActive(false);
        _UIBackgroundVFX.gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        if (_firstLogin)
        {
            _loginControl.SetActive(true);
            _firstLogin = false;
        }
        else
            _enterRoomControl.SetActive(true);
        _UIBackgroundVFX.gameObject.SetActive(true);
    }
}
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _loginUI;
    [SerializeField] private GameObject _nameUI;
    [SerializeField] private InputField _roomName;
    [SerializeField] private InputField _nameInputField;
    [SerializeField] private InputField _emailNameInputField;
    [SerializeField] private InputField _passwordInputField;
    [SerializeField] private Text _errorText;

    private void Start()
    {
        _nameInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _emailNameInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _passwordInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        PlayFabSettings.TitleId = "9B23C";

        if (PhotonNetwork.IsConnected != true)
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
        _nameUI.SetActive(true);
    }

    public void PlayButton()
    {
        if (_nameInputField.text == string.Empty)
        {
            _errorText.text = "The name cannot be empty!";
            return;
        }

        if (_emailNameInputField.text == string.Empty)
        {
            _errorText.text = "The email cannot be empty!";
            return;
        }

        if (_passwordInputField.text == string.Empty)
        {
            _errorText.text = "The password cannot be empty!";
            return;
        }

        PhotonNetwork.NickName = _nameInputField.text;
        var request = new LoginWithEmailAddressRequest
            {Email = _emailNameInputField.text, Password = _passwordInputField.text};
        PlayFabClientAPI.LoginWithEmailAddress(request,
            loginResult =>
            {
                _nameUI.SetActive(false);
                _loginUI.SetActive(true);
                Debug.Log(loginResult.PlayFabId);
            },
            playfabError =>
            {
                _errorText.text = playfabError.ErrorMessage;
                Debug.LogError(playfabError.ErrorMessage);
            });
    }

    public void JoinOrCreateButton()
    {
        if (_roomName.text.Length < 2)
            return;

        _loginUI.SetActive(false);

        var options = new RoomOptions {MaxPlayers = 10};
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, default);
    }

    public void ShowLobbyButton()
    {
        _loginUI.SetActive(false);
        //LobbyControl.lobby.Show();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Scene01");
    }

    //for back to menu
    public void changeScene()
    {
        PhotonNetwork.LoadLevel("menu");
        PhotonNetwork.Disconnect();
    }

    //for back to menu in game
    public void changeSceneInGame()
    {
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LoadLevel("menu");
        //PhotonNetwork.Disconnect();
    }
}
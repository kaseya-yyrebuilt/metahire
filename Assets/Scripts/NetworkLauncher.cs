using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public GameObject loginUI;
    public GameObject nameUI;
    public InputField roomName;
    public InputField nameInputField;
    public InputField emailNameInputField;
    public InputField passwordInputField;
    public Text errorText;

    private void Start()
    {
        nameInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        emailNameInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        passwordInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        PlayFabSettings.TitleId = "9B23C";

        if (PhotonNetwork.IsConnected != true)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // PhotonNetwork.Disconnect();
            //PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        nameUI.SetActive(true);
    }

    public void PlayButton()
    {
        if (nameInputField.text == string.Empty)
        {
            errorText.text = "The name cannot be empty!";
            return;
        }
        else if (emailNameInputField.text == string.Empty)
        {
            errorText.text = "The email cannot be empty!";
            return;
        }
        else if (passwordInputField.text == string.Empty)
        {
            errorText.text = "The password cannot be empty!";
            return;
        }

        PhotonNetwork.NickName = nameInputField.text;
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest() { Email = emailNameInputField.text, Password = passwordInputField.text };
        PlayFabClientAPI.LoginWithEmailAddress(request,
        loginResult =>
        {
            nameUI.SetActive(false);
            loginUI.SetActive(true);
            Debug.Log(loginResult.PlayFabId);
        },
        playfabError =>
        {
            errorText.text = playfabError.ErrorMessage;
            Debug.LogError(playfabError.ErrorMessage);
        });
    }

    public void JoinOrCreateButton()
    {
        if (roomName.text.Length < 2)
            return;

        loginUI.SetActive(false);

        RoomOptions options = new RoomOptions { MaxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, default);
    }

    public void ShowLobbyButton()
    {
        loginUI.SetActive(false);
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class EnterRoomControl : MonoBehaviourPunCallbacks
{
    private CanvasGroup thisCanvasGroup;
    [SerializeField]
    private InputField roomNameInputField;
    [SerializeField]
    private Button joinButton;
    [SerializeField]
    private Button lobbyButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Text errorText;
    [SerializeField]
    private LoginControl loginControl;
    [SerializeField]
    private LobbyControl lobbyControl;
    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        roomNameInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        joinButton.onClick.AddListener(() =>
        {
            if (roomNameInputField.text == string.Empty)
            {
                errorText.text = "The room name cannot be empty!";
                return;
            }

            RoomOptions options = new RoomOptions { MaxPlayers = 10 };
            PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, options, TypedLobby.Default);
        });

        lobbyButton.onClick.AddListener(() =>
        {
            SetActive(false);
            errorText.text = string.Empty;
            lobbyControl.SetActive(true);
        });

        backButton.onClick.AddListener(() =>
        {
            SetActive(false);
            errorText.text = string.Empty;
            loginControl.SetActive(true);
        });

        SetActive(false);
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Scene01", LoadSceneMode.Single);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = message;
    }
    public void SetActive(bool isActive)
    {
        thisCanvasGroup.alpha = isActive ? 1f : 0f;
        thisCanvasGroup.blocksRaycasts = isActive;
    }
}

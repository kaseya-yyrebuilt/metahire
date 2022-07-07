using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatControl : MonoBehaviour, IChatClientListener
{
    private string _channelName;
    private ChatClient _chatClient;

    [SerializeField] private Transform _contentT;
    [SerializeField] private TMP_InputField _messageInputField;
    [SerializeField] private ScrollRect _messageScrollRect;
    [SerializeField] private GameObject _messageTextP;
    [SerializeField] private TextMeshProUGUI _pingText;
    [SerializeField] private Canvas _thisCanvas;

    private void Start()
    {
        if (!_thisCanvas) _thisCanvas = GetComponent<Canvas>();

        _messageInputField.onSubmit.AddListener(message =>
        {
            if (!string.IsNullOrEmpty(message))
            {
                _chatClient.PublishMessage(_channelName, message);
                _messageInputField.text = string.Empty;
            }
        });

        _messageInputField.onSelect.AddListener(_ => PlayerController.PlayerControlsEnabled = false);
        _messageInputField.onDeselect.AddListener(_ => PlayerController.PlayerControlsEnabled = true);
    }

    public void OnEnable()
    {
        _chatClient = new ChatClient(this);
        var appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        //_chatClient.ChatRegion = "us";
        _chatClient.Connect(appSettings.AppIdChat, appSettings.AppVersion,
            new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
        _channelName = PhotonNetwork.CloudRegion ?? "" + PhotonNetwork.CurrentRoom.Name;

        _thisCanvas.enabled = true;
    }

    private void Update()
    {
        _chatClient?.Service(); // Must be called regularly to keep connection between client and server alive
        if (_pingText) _pingText.text = $"Ping: {PhotonNetwork.GetPing()}";
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case DebugLevel.ERROR:
                Debug.LogError(message);
                break;

            case DebugLevel.WARNING:
                Debug.LogWarning(message);
                break;

            case DebugLevel.INFO:
                Debug.Log(message);
                break;
        }
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat OnDisconnected");
    }

    public void OnConnected()
    {
        _thisCanvas.enabled = true;
        _chatClient.Subscribe(_channelName);
        Debug.Log("OnConnected");
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (var i = 0; i < senders.Length; i++)
            CreateMessage(senders[i], messages[i].ToString());
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    private void CreateMessage(string sender, string message)
    {
        var messageText = Instantiate(_messageTextP).GetComponent<TextMeshProUGUI>();
        messageText.text =
            $"{DateTime.Now.ToString("[hh:mm:ss]")}<color={(sender == PhotonNetwork.LocalPlayer.NickName ? "red" : "green")}>{sender}:</color>{message}";
        messageText.transform.SetParent(_contentT, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_messageScrollRect.GetComponent<RectTransform>());
        _messageScrollRect.verticalNormalizedPosition = 0f;
    }

    private void DestroyAllMessages()
    {
        // Deconstruction happens at the end of the frame
        for (int i = _contentT.childCount-1; i >= 0 ; i--)
            Destroy(_contentT.GetChild(i).gameObject);
    }

    public void LeaveRoom()
    {
        DestroyAllMessages();
        PhotonNetwork.LeaveRoom();
    }
}
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatControl : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private string channelName;
    [SerializeField]
    private CanvasGroup thisCanvasGroup;
    [SerializeField]
    private ScrollRect messageScrollRect;
    [SerializeField]
    private Transform contentT;
    [SerializeField]
    private GameObject messageTextP;
    [SerializeField]
    private TMP_InputField messageInputField;
    [SerializeField]
    private TextMeshProUGUI PingText;

    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup.alpha = 0f;
        thisCanvasGroup.blocksRaycasts = false;

        messageInputField.onEndEdit.AddListener(message =>
        {
            if (!string.IsNullOrEmpty(message))
            {
                chatClient.PublishMessage(channelName, message);
                messageInputField.text = string.Empty;
            }
        });

        chatClient = new ChatClient(this);
        Photon.Realtime.AppSettings appSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        chatClient.ChatRegion = "asia";
        chatClient.Connect(appSettings.AppIdChat, appSettings.AppVersion, new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
        channelName = PhotonNetwork.CloudRegion + PhotonNetwork.CurrentRoom.Name;
        messageInputField.onSelect.AddListener(_ => PlayerController.playerControlsEnabled = false);
        messageInputField.onDeselect.AddListener(_ => PlayerController.playerControlsEnabled = true);
    }
    // Update is called once per frame
    void Update()
    {
        chatClient?.Service();
        if (PingText) PingText.text = $"Ping: {PhotonNetwork.GetPing()}";
    }
    private void CreateMessage(string sender, string message)
    {
        TextMeshProUGUI messageText = Instantiate(messageTextP).GetComponent<TextMeshProUGUI>();
        messageText.text = $"{DateTime.Now.ToString("[hh:mm:ss]")}<color={(sender == PhotonNetwork.LocalPlayer.NickName ? "red" : "green")}>{sender}:</color>{message}";
        messageText.transform.SetParent(contentT, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageScrollRect.GetComponent<RectTransform>());
        messageScrollRect.verticalNormalizedPosition = 0f;
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
        Debug.Log("OnDisconnected");
    }
    public void OnConnected()
    {
        thisCanvasGroup.alpha = 1f;
        thisCanvasGroup.blocksRaycasts = true;
        chatClient.Subscribe(channelName);
        Debug.Log("OnConnected");
    }
    public void OnChatStateChange(ChatState state)
    {

    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            CreateMessage(senders[i], messages[i].ToString());
        }
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
}

using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
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
    private InputField messageInputField;
    [SerializeField]
    private Text PingText;

    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup.alpha = 0f;
        thisCanvasGroup.blocksRaycasts = false;

        messageInputField.onEndEdit.AddListener(message =>
        {
            if (message != string.Empty)
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
    }
    // Update is called once per frame
    void Update()
    {
        chatClient?.Service();
        if (messageInputField.GetComponent<InputField>().isFocused == true)
        {
            PlayerController.playerControlsEnabled = false;
        }
        else
        {
            PlayerController.playerControlsEnabled = true;
        }
        if (PingText) PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }
    private void CreateMessage(string sender, string message)
    {
        Text messageText = Instantiate<GameObject>(messageTextP).GetComponent<Text>();
        messageText.text = $"{DateTime.Now.ToString("[hh:mm:ss]")}<color={(sender == PhotonNetwork.LocalPlayer.NickName ? "red" : "green")}>{sender}:</color>{message}";
        messageText.transform.SetParent(contentT, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageScrollRect.GetComponent<RectTransform>());
        messageScrollRect.verticalNormalizedPosition = 0f;
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
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

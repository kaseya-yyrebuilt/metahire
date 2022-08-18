using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoomControl : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    [SerializeField] private int _roomCapacity = 4;
    [Header("Controllers")]
    [SerializeField] private LobbyControl _lobbyControl;
    [SerializeField] private LoginControl _loginControl;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private Button _joinButton;
    [SerializeField] private Button _lobbyButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TextMeshProUGUI _errorText;

    [SerializeField] private Canvas _thisCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        if (!_thisCanvas) _thisCanvas = GetComponent<Canvas>();
        _roomNameInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _joinButton.onClick.AddListener(() =>
        {
            if (_roomNameInputField.text == string.Empty)
            {
                _errorText.text = "The room name cannot be empty!";
                return;
            }

            var options = new RoomOptions {MaxPlayers = System.Convert.ToByte(_roomCapacity)};
            PhotonNetwork.JoinOrCreateRoom(_roomNameInputField.text, options, TypedLobby.Default);
        });

        _lobbyButton.onClick.AddListener(() =>
        {
            if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
            SetActive(false);
            _errorText.text = string.Empty;
            _lobbyControl.SetActive(true);
        });

        _backButton.onClick.AddListener(() =>
        {
            SetActive(false);
            _errorText.text = string.Empty;
            _loginControl.SetActive(true);
        });

        SetActive(false);
    }

    private void OnValidate()
    {
        if (!_lobbyControl) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_lobbyControl)} is not defined");
        if (!_loginControl) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_loginControl)} is not defined");
        if (!_roomNameInputField) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_roomNameInputField)} is not defined");
        if (!_joinButton) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_joinButton)} is not defined");
        if (!_lobbyButton) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_lobbyButton)} is not defined");
        if (!_backButton) Debug.LogWarning($"{name}:{nameof(EnterRoomControl)}.{nameof(_backButton)} is not defined");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _errorText.text = message;
    }

    public void SetActive(bool isActive)
    {
        _thisCanvas.enabled = isActive;
    }
}
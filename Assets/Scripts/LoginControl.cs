using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginControl : MonoBehaviour
{
    [SerializeField] private EnterRoomControl _enterRoomControl;
    [SerializeField] private RegisterControl _registerControl;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private TextMeshProUGUI _errorText;
    [SerializeField] private Canvas _thisCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        if (!_thisCanvas) _thisCanvas = GetComponent<Canvas>();

        _emailInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _passwordInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _loginButton.onClick.AddListener(() =>
        {
            if (_emailInputField.text == string.Empty)
            {
                _errorText.text = "The email cannot be empty!";
                return;
            }

            if (_passwordInputField.text == string.Empty)
            {
                _errorText.text = "The password cannot be empty!";
                return;
            }

            var loginRequest = new LoginWithEmailAddressRequest
            {
                Email = _emailInputField.text,
                Password = _passwordInputField.text
            };
            PlayFabClientAPI.LoginWithEmailAddress(loginRequest,
                loginResult =>
                {
                    var getAccountInfoRequest = new GetAccountInfoRequest
                    {
                        PlayFabId = loginResult.PlayFabId
                    };
                    PlayFabClientAPI.GetAccountInfo(getAccountInfoRequest,
                        getAccountInfoResult =>
                        {
                            PhotonNetwork.NickName = getAccountInfoResult.AccountInfo.Username;
                            _errorText.text = string.Empty;
                            SetActive(false);
                            _enterRoomControl.SetActive(true);
                            Debug.Log($"Logged in as {getAccountInfoResult.AccountInfo.Username}");
                        },
                        getAccountInfoError =>
                        {
                            _errorText.text = getAccountInfoError.ErrorMessage;
                            Debug.LogError(getAccountInfoError.ErrorMessage);
                        });
                },
                loginError =>
                {
                    _errorText.text = loginError.ErrorMessage;
                    Debug.LogError(loginError.ErrorMessage);
                });
        });

        _registerButton.onClick.AddListener(() =>
        {
            _errorText.text = string.Empty;
            SetActive(false);
            _registerControl.SetActive(true);
        });

        PlayFabSettings.TitleId = "9B23C";

        SetActive(false);
    }

    private void OnValidate()
    {
        if (!_enterRoomControl) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_enterRoomControl)} is not defined");
        if (!_registerControl) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_registerControl)} is not defined");
        if (!_emailInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_emailInputField)} is not defined");
        if (!_passwordInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_passwordInputField)} is not defined");
        if (!_loginButton) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_loginButton)} is not defined");
        if (!_registerButton) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_registerButton)} is not defined");
        if (!_errorText) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_errorText)} is not defined");
    }

    public void SetActive(bool isActive)
    {
        _thisCanvas.enabled = isActive;
    }
}
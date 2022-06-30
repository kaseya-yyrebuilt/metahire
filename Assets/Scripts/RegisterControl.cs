using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterControl : MonoBehaviour
{
    [SerializeField] private LoginControl _loginControl;
    [SerializeField] private TMP_InputField _loginEmailInputField;

    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _confirmInputField;

    [SerializeField] private Button _registerButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TextMeshProUGUI _errorText;

    [SerializeField] private Canvas _thisCanvas;

    private void Start()
    {
        if (!_thisCanvas) _thisCanvas = GetComponent<Canvas>();

        _emailInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _usernameInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _passwordInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _confirmInputField.onValueChanged.AddListener(input => { _errorText.text = string.Empty; });

        _registerButton.onClick.AddListener(() =>
        {
            string email = _emailInputField.text,
                username = _usernameInputField.text,
                password = _passwordInputField.text,
                confirm = _confirmInputField.text;

            if (string.IsNullOrWhiteSpace(email))
            {
                _errorText.text = "The email cannot be empty!";
                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                _errorText.text = "The username cannot be empty!";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _errorText.text = "The password cannot be empty!";
                return;
            }

            if (string.IsNullOrWhiteSpace(confirm))
            {
                _errorText.text = "The confirm cannot be empty!";
                return;
            }

            if (password.Length < 6)
            {
                _errorText.text = "The password should not be less than 6 letters!";
                return;
            }

            if (confirm != password)
            {
                _errorText.text = "The passwords entered are different!";
                return;
            }

            var registerRequest = new RegisterPlayFabUserRequest
            {
                Email = email,
                Password = password,
                Username = username
            };

            PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
                registerResult =>
                {
                    SetActive(false);
                    _errorText.text = string.Empty;
                    _loginControl.SetActive(true);
                    _loginEmailInputField.text = email;
                },
                registerError =>
                {
                    Debug.LogWarning($"{registerError.Error}");
                    _errorText.text = registerError.ErrorMessage;
                    Debug.LogError(registerError.ErrorMessage);
                });
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
        if (!_loginControl) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_loginControl)} is not defined");
        if (!_loginEmailInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_loginEmailInputField)} is not defined");
        if (!_emailInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_emailInputField)} is not defined");
        if (!_passwordInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_passwordInputField)} is not defined");
        if (!_usernameInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_usernameInputField)} is not defined");
        if (!_confirmInputField) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_confirmInputField)} is not defined");
        if (!_registerButton) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_registerButton)} is not defined");
        if (!_backButton) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_backButton)} is not defined");
        if (!_errorText) Debug.LogWarning($"{name}:{nameof(PlayerController)}.{nameof(_errorText)} is not defined");
    }

    public void SetActive(bool isActive)
    {
        _thisCanvas.enabled = isActive;
    }
}
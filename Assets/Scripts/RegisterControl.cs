using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class RegisterControl : MonoBehaviour
{
    private CanvasGroup thisCanvasGroup;
    [SerializeField]
    private InputField emailInputField;
    [SerializeField]
    private InputField usernameInputField;
    [SerializeField]
    private InputField passwordInputField;
    [SerializeField]
    private InputField confirmInputField;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Text errorText;
    [SerializeField]
    private LoginControl loginControl;
    [SerializeField]
    private InputField loginEmailInputField;
    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        emailInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        usernameInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        passwordInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        confirmInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        registerButton.onClick.AddListener(() =>
        {
            string email = emailInputField.text, 
                username = usernameInputField.text, 
                password = passwordInputField.text, 
                confirm  = confirmInputField.text;

            if (string.IsNullOrWhiteSpace(email))
            {
                errorText.text = "The email cannot be empty!";
                return;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                errorText.text = "The username cannot be empty!";
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                errorText.text = "The password cannot be empty!";
                return;
            }
            if (string.IsNullOrWhiteSpace(confirm))
            {
                errorText.text = "The confirm cannot be empty!";
                return;
            }
            if (password.Length < 6)
            {
                errorText.text = "The password should not be less than 6 letters!";
                return;
            }
            if (confirm != password)
            {
                errorText.text = "The passwords entered are different!";
                return;
            }

            RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest()
            {
                Email = email,
                Password = password,
                Username = username
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            registerResult =>
            {
                SetActive(false);
                errorText.text = string.Empty;
                loginControl.SetActive(true);
                loginEmailInputField.text = email;
            },
            loginError =>
            {
                Debug.LogWarning($"{loginError.Error}");
                errorText.text = loginError.ErrorMessage;
                Debug.LogError(loginError.ErrorMessage);
            });
        });

        backButton.onClick.AddListener(() =>
        {
            SetActive(false);
            errorText.text = string.Empty;
            loginControl.SetActive(true);
        });

        SetActive(false);
    }
    public void SetActive(bool isActive)
    {
        thisCanvasGroup.alpha = isActive ? 1f : 0f;
        thisCanvasGroup.blocksRaycasts = isActive;
    }
}

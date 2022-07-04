using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

public class LoginControl : MonoBehaviour
{
    private CanvasGroup thisCanvasGroup;
    [SerializeField]
    private InputField emailInputField;
    [SerializeField]
    private InputField passwordInputField;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Text errorText;
    [SerializeField]
    private RegisterControl registerControl;
    [SerializeField]
    private EnterRoomControl enterRoomControl;
    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        emailInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        passwordInputField.onValueChanged.AddListener(input =>
        {
            errorText.text = string.Empty;
        });

        loginButton.onClick.AddListener(() =>
        {
            if (emailInputField.text == string.Empty)
            {
                errorText.text = "The email cannot be empty!";
                return;
            }
            else if (passwordInputField.text == string.Empty)
            {
                errorText.text = "The password cannot be empty!";
                return;
            }

            LoginWithEmailAddressRequest loginRequest = new LoginWithEmailAddressRequest()
            {
                Email = emailInputField.text,
                Password = passwordInputField.text
            };
            PlayFabClientAPI.LoginWithEmailAddress(loginRequest,
            loginResult =>
            {
                GetAccountInfoRequest getAccountInfoRequest = new GetAccountInfoRequest()
                {
                    PlayFabId = loginResult.PlayFabId
                };
                PlayFabClientAPI.GetAccountInfo(getAccountInfoRequest,
                getAccountInfoResult =>
                {
                    PhotonNetwork.NickName = getAccountInfoResult.AccountInfo.Username;
                    errorText.text = string.Empty;
                    SetActive(false);
                    enterRoomControl.SetActive(true);
                    Debug.Log(getAccountInfoResult.AccountInfo.Username);
                },
                getAccountInfoError =>
                {
                    errorText.text = getAccountInfoError.ErrorMessage;
                    Debug.LogError(getAccountInfoError.ErrorMessage);
                });
            },
            loginError =>
            {
                errorText.text = loginError.ErrorMessage;
                Debug.LogError(loginError.ErrorMessage);
            });
        });

        registerButton.onClick.AddListener(() =>
        {
            errorText.text = string.Empty;
            SetActive(false);
            registerControl.SetActive(true);
        });

        PlayFabSettings.TitleId = "9B23C";

        SetActive(false);
    }
    public void SetActive(bool isActive)
    {
        thisCanvasGroup.alpha = isActive ? 1f : 0f;
        thisCanvasGroup.blocksRaycasts = isActive;
    }
}

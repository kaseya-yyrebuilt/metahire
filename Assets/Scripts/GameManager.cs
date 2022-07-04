using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject readyButton;
    public GameObject RubyButton;
    public GameObject RobotButton;
    public GameObject Canvas;
    private string character = "Ruby";
    public GameObject RubyFrame;
    public GameObject RobotFrame;
    public Text PingText;

    private void Start()
    {
        RubyFrame.SetActive(true);
        RobotFrame.SetActive(false);
    }

    private void Update()
    {
        PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void ChooseRuby()
    {
        character = "Ruby";
        RubyFrame.SetActive(true);
        RobotFrame.SetActive(false);
    }

    public void ChooseRobot()
    {
        character = "Robot";
        RubyFrame.SetActive(false);
        RobotFrame.SetActive(true);
    }

    public void ReadyToPlay()
    {
        Canvas.SetActive(false);
        PhotonNetwork.Instantiate(character, new Vector3(1, 1, 0), Quaternion.identity, 0);
    }
}

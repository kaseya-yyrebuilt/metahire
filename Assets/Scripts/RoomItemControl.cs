using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomItemControl : MonoBehaviour
{
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text gamersCountText;
    [SerializeField]
    private Button joinButton;
    public void Init(string name, int gamersCount)
    {
        nameText.text = name;
        gamersCountText.text = $"{gamersCount} / 4";
        joinButton.onClick.AddListener(() =>
        {
            PhotonNetwork.JoinRoom(name);
        });
    }
    public void SetGamersCount(int gamersCount)
    {
        gamersCountText.text = $"{gamersCount} / 4";
    }
}

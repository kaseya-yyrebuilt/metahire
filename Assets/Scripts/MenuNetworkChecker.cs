using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuNetworkChecker : MonoBehaviour
{
    private void Start()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
           
        }
    }
}

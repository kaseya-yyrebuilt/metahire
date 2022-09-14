using Photon.Pun;
using UnityEngine;

public class MenuNetworkChecker : MonoBehaviour
{
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
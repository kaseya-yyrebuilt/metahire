using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    private CanvasGroup thisCanvasGroup;
    [SerializeField]
    private Transform roomsListContentT;
    [SerializeField]
    private GameObject roomItemImageP;
    private Dictionary<string, RoomItemControl> roomItemsDic = new Dictionary<string, RoomItemControl>();
    [SerializeField]
    private Button returnButton;
    [SerializeField]
    private EnterRoomControl enterRoomControl;
    // Start is called before the first frame update
    void Start()
    {
        thisCanvasGroup = this.gameObject.AddComponent<CanvasGroup>();

        returnButton.onClick.AddListener(() =>
        {
            SetActive(false);
            enterRoomControl.SetActive(true);
        });

        SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomList)
        {
            if (item.RemovedFromList || item.PlayerCount == item.MaxPlayers || !item.IsOpen)
            {
                if (roomItemsDic.ContainsKey(item.Name))
                {
                    Destroy(roomItemsDic[item.Name].gameObject);
                    roomItemsDic.Remove(item.Name);
                }
            }
            else
            {
                if (roomItemsDic.ContainsKey(item.Name))
                {
                    roomItemsDic[item.Name].SetGamersCount(item.PlayerCount);
                }
                else
                {
                    Transform roomItemT = Instantiate<GameObject>(roomItemImageP).transform;
                    roomItemT.SetParent(roomsListContentT, false);
                    RoomItemControl roomItemControl = roomItemT.GetComponent<RoomItemControl>();
                    roomItemControl.Init(item.Name, item.PlayerCount);
                    roomItemsDic.Add(item.Name, roomItemControl);
                }
            }
        }
    }
    public void SetActive(bool isActive)
    {
        thisCanvasGroup.alpha = isActive ? 1f : 0f;
        thisCanvasGroup.blocksRaycasts = isActive;
    }
}

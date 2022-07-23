using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ToggleGroup _toggle;
    [SerializeField] private Canvas _inGameDisplay;
    [SerializeField] private ChatControl _chatControl;

    private string _character;

    private void Start()
    {
        if (!_canvas) _canvas = GetComponent<Canvas>();
        //_canvas.enabled = true;
    }

    private void getCharacter()
    {
        if (!_toggle || !_toggle.AnyTogglesOn())
        {
            Debug.LogWarning("Toggle is not available");
            return;
        }

        var t = _toggle.ActiveToggles().ToArray();
        if (t.Length != 1)
        {
            Debug.LogWarning("More than 1 _character selected");
        }
        else
        {
            var cname = t[0].gameObject.name;
            if (cname.Contains("Ruby")) _character = "Ruby";
            else if (cname.Contains("MrClock")) _character = "Robot";
        }
    }

    public void ReadyToPlay()
    {
        _canvas.enabled = false;
        getCharacter();
        if (string.IsNullOrEmpty(_character)) _character = "Ruby";
        _inGameDisplay.enabled = true;
        _chatControl.enabled = true;
        var respawn = GameObject.FindGameObjectWithTag("Respawn");
        var respawnPos = respawn ? respawn.transform.position + new Vector3(0, -1f, 0) : new Vector3(1, 1, 0);
        PhotonNetwork.Instantiate(_character, respawnPos, Quaternion.identity);
    }

    public override void OnJoinedRoom()
    {
        _canvas.enabled = true;
    }

    public override void OnConnectedToMaster()
    {
        _chatControl.enabled = false;
        _inGameDisplay.enabled = false;
    }
}
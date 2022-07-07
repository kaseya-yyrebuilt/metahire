using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _gamersCountText;
    [SerializeField] private Button _joinButton;

    private int _capacity;

    private void OnValidate()
    {
        if (!_nameText) Debug.LogWarning($"{name}:{nameof(RoomItemControl)}.{nameof(_nameText)} is not defined");
        if (!_gamersCountText)
            Debug.LogWarning($"{name}:{nameof(RoomItemControl)}.{nameof(_gamersCountText)} is not defined");
        if (!_joinButton) Debug.LogWarning($"{name}:{nameof(RoomItemControl)}.{nameof(_joinButton)} is not defined");
    }

    public void Init(string name, int gamersCount, int capacity)
    {
        _nameText.text = name;
        _gamersCountText.text = $"{gamersCount} / {_capacity}";
        _joinButton.onClick.AddListener(() => { PhotonNetwork.JoinRoom(name); });

        _capacity = capacity;
    }

    public void SetGamersCount(int gamersCount) => _gamersCountText.text = $"{gamersCount} / {_capacity}";
}
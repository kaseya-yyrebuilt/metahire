using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tipImage;

    [SerializeField]
    private TextMeshProUGUI enterRoomImage;

    [SerializeField]
    private Collider2D voiceTriggerArea;

    private bool entered = false;

    private void Start()
    {
        if (!voiceTriggerArea) voiceTriggerArea = GetComponent<Collider2D>();
        tipImage.enabled = true;
        enterRoomImage.enabled = false;
    }

    private void OnValidate()
    {
        if (!tipImage) Debug.LogWarning("Missing tip for triggering voice chat");
        if (!enterRoomImage) Debug.LogWarning("Missing tip for chatting");
    }

    private void Update()
    {
        if (entered && Input.GetKeyDown(KeyCode.E))
            ToggleDialog();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetPhotonView().IsMine)
            entered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetPhotonView().IsMine)
        {
            ToggleDialog(false);
            entered = false;
        }
    }

    /// <summary>
    /// Toggle the status of microphone and speaker
    /// </summary>
    public void ToggleDialog(bool value)
    {
        tipImage.enabled = value;
        enterRoomImage.enabled = !value;
        PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = !value;
        foreach (Speaker item in Component.FindObjectsOfType<Speaker>())
        {
            item.enabled = !value;
        }
    }
    public void ToggleDialog()
    {
        ToggleDialog(!tipImage.enabled);
    }
}

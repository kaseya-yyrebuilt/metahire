using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Canvas Canvas;

    [SerializeField]
    private ToggleGroup toggle;

    [SerializeField]
    private GameObject NextCanvas;

    private string character;

    private void getCharacter()
    {
        if (!toggle || !toggle.AnyTogglesOn())
        {
            Debug.LogWarning("Toggle is not available");
            return;
        }
        
        Toggle[] t = toggle.ActiveToggles().Cast<Toggle>().ToArray();
        if (t.Length != 1)
        {
            Debug.LogWarning("More than 1 character selected");
        }
        else
        {
            string cname = t[0].gameObject.name;
            if (cname.Contains("Ruby")) character = "Ruby";
            else if (cname.Contains("MrClock")) character = "Robot";
        }
    }

    public void ReadyToPlay()
    {
        Canvas.enabled = false;
        getCharacter();
        if (string.IsNullOrEmpty(character)) character = "Ruby";
        PhotonNetwork.Instantiate(character, new Vector3(1, 1, 0), Quaternion.identity, 0);
        NextCanvas.SetActive(true);
    }
}

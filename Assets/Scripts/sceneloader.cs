using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{
    private void Update()
    {
        // Create a temporary reference to the current scene.
        //Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        //string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.F))
        {
            PhotonNetwork.LoadLevel("Scene01");
            SceneManager.LoadSceneAsync("mainlogin", LoadSceneMode.Additive);
        }
    }
}
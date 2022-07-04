using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{
    [System.Obsolete]
    void Update()
    {
        // Create a temporary reference to the current scene.
        //Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        //string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Application.LoadLevel("mainlogin");
        }
        else
        {

        }
    }
}

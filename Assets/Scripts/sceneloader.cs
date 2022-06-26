using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneloader : MonoBehaviour
{

    void Update()
    {
        // Create a temporary reference to the current scene.
        //Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        //string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("mainlogin");
        }
        else
        {

        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
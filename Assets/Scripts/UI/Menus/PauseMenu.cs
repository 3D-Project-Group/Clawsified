using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ContinueGame()
    {
        this.gameObject.SetActive(false); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    [Header("Start Screen")]
    public bool waitingForClick = true;

    [SerializeField]private GameObject startScreen;
    [SerializeField]private GameObject menusScreen;

    void Update()
    {
        if(waitingForClick && Input.anyKeyDown)
        {
            waitingForClick = false;
            LoadMenu(startScreen, menusScreen);
        }
    }

    public void LoadMenu(GameObject menuToUnload, GameObject menuToLoad)
    {
        menuToUnload.SetActive(false);
        menuToLoad.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Revive()
    {
        if (GameInfo.Fighting_Boss)
        {
            LoadScene("BossScene");
        }
        else
        {
            LoadScene("Lab");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    [Header("Start Screen")]
    public bool waitingForClick = true;
    [SerializeField] private AudioSource bgMusic;

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject menusScreen;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;    
        Cursor.visible = true;    
    }

    void Update()
    {
        if(startScreen != null && waitingForClick && Input.anyKeyDown)
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
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        Time.timeScale = 1.0f;
        transitionAnimator.SetTrigger("Start");

        do
        {
            bgMusic.volume -= Time.deltaTime;
            yield return null;
        } while (bgMusic.volume > 0);

        yield return new WaitForSeconds(1f);
        // yield return null;

        GameInfo.SceneToLoad = sceneName;
        GameInfo.SceneToUnload = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync("LoadingScreen");
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

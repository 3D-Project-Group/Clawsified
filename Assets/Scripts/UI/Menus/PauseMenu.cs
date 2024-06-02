using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameManager gameManager;
    public void ContinueGame()
    {
        this.gameObject.SetActive(false); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        gameManager.UnpauseGameSounds();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        Time.timeScale = 1.0f;
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(2f);

        GameInfo.SceneToLoad = sceneName;
        GameInfo.SceneToUnload = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScreen");
    }
}

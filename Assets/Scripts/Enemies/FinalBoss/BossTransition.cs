using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTransition : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    IEnumerator Transition(string sceneName)
    {
        Time.timeScale = 1.0f;
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(2f);

        GameInfo.SceneToLoad = sceneName;
        GameInfo.SceneToUnload = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Transition("BossScene"));
            GameInfo.Fighting_Boss = true;
        }
    }
}



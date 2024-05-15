using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(GameInfo.SceneToLoad);

        while (!operation.isDone)
        {
            print(operation.progress.ToString() + " || Operation Progress");
            float progressValue = Mathf.Clamp01(operation.progress);
            progressBar.value = progressValue;

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.UnloadSceneAsync(GameInfo.SceneToUnload); 
    }
}

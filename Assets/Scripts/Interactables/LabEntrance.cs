using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabEntrance : Interact
{
    [SerializeField] private Animator anim;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private AudioSource gateOpen;

    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public override void Interaction()
    {
        anim.SetBool("Open", true);
    }

    void OpenSound()
    {
        gateOpen.Play();
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

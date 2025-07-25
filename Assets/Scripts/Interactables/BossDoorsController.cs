using UnityEngine;

public class BossDoorsController : MonoBehaviour
{
    public GameObject[] buttons;
    [Space]
    public GameObject[] objsToUnactivate;

    public void PressButton()
    {
        //Verify if all pressed
        bool allPressed = true;
        foreach (GameObject button in buttons)
        {
            if(button.GetComponent<BossDoorsButton>().activate)
                allPressed = false;
        }

        //Unactivate obj
        if (allPressed)
        {
            foreach (GameObject obj in objsToUnactivate)
            {
                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool("Open", true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}

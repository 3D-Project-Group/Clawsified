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
            if(!button.GetComponent<BossDoorsButton>().pressed)
                allPressed = false;
        }

        //Unactivate obj
        if (allPressed)
        {
            foreach (GameObject obj in objsToUnactivate)
            {
                obj.SetActive(false);
            }
        }
    }
}

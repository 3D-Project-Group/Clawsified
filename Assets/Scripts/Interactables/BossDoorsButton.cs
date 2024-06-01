using UnityEngine;

public class BossDoorsButton : Interact
{
    [SerializeField] private string tutorialTitle;
    [SerializeField] private string tutorialText;
    [SerializeField] private int tutorialImgIndex;
    [SerializeField] private Light[] lightsToChange;

    public override void Update()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 2 && activate)
        {
            interactableImage.SetActive(true);
        }
        else interactableImage.SetActive(false);
    }

    public override void Interaction()
    {
        base.Interaction();
        if (!GameInfo.bossButtonsTutorial)
        {
            GameInfo.bossButtonsTutorial = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddPopupToQueue(new Popup(tutorialTitle, tutorialText, tutorialImgIndex));
        }

        foreach (Light light in lightsToChange)
        {
            light.color = Color.green;
        }

        GetComponentInParent<BossDoorsController>().PressButton();
        // this.gameObject.SetActive(false);
    }
}

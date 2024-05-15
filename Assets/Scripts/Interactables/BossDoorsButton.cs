using UnityEngine;

public class BossDoorsButton : Interact
{
    [SerializeField] private string tutorialTitle;
    [SerializeField] private string tutorialText;
    [SerializeField] private int tutorialImgIndex;
    public override void Interaction()
    {
        base.Interaction();
        if (!GameInfo.bossButtonsTutorial)
        {
            GameInfo.bossButtonsTutorial = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().AddPopupToQueue(new Popup(tutorialTitle, tutorialText, tutorialImgIndex));
        }

        GetComponentInParent<BossDoorsController>().PressButton();
        this.gameObject.SetActive(false);
    }
}

using UnityEngine;

public class BossDoorsButton : Interact
{
    public override void Interaction()
    {
        base.Interaction();
        activate = true;
        GetComponentInParent<BossDoorsController>().PressButton();
        this.gameObject.SetActive(false);
    }
}

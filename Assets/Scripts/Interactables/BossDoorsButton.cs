using UnityEngine;

public class BossDoorsButton : Interact
{
    public bool pressed = false;
    public override void Interaction()
    {
        base.Interaction();
        pressed = true;
        GetComponentInParent<BossDoorsController>().PressButton();
        this.gameObject.SetActive(false);
    }
}

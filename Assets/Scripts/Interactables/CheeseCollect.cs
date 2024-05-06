using UnityEngine;

public class CheeseCollect : Interact
{
    private PlayerController controller;

    private new void Start()
    {
        base.Start();
        controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Interaction()
    {
        base.Interaction();
        controller.AddCheese(1);
    }

}

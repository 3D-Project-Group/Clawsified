using UnityEngine;

public class ColorPuzzleButton : Interact
{
    [SerializeField] private ColorPuzzleTube tube;

    public override void Interaction()
    {
        base.Interaction();
        if ((int)tube.currentColor < 2)
        {
            tube.SetValue(tube.currentColor + 1);
        }
        else
        {
            tube.SetValue(0);
        }
    }
}
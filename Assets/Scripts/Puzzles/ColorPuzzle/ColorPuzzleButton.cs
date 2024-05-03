using UnityEngine;

public class ColorPuzzleButton : Interact
{
    [SerializeField] private int tubeID = 0;
    public override void Interaction()
    {
        base.Interaction();
        GetComponentInParent<ColorPuzzleController>().SetTubeColor(tubeID);
    }
}
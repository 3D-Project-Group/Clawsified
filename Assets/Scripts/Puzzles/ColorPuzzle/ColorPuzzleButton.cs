using UnityEngine;

public class ColorPuzzleButton : Interact
{
    [SerializeField] private int tubeID = 0;
    [SerializeField] private ColorPuzzleController tubeController ;

    new void Start()
    {
        base.Start();
        tubeController = GetComponentInParent<ColorPuzzleController>();
    }
    public override void Update()
    {
        base.Update();
        if (!tubeController.isActive)
        {
            activate = false;
        }
    }
    public override void Interaction()
    {
        tubeController.SetTubeColor(tubeID);
    }
}
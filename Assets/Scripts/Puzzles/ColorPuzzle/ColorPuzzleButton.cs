using UnityEngine;

public class ColorPuzzleButton : Interact
{
    [SerializeField] private int tubeID = 0;
    [SerializeField] private ColorPuzzleController tubeController;
    private AudioSource buttonPressSound;

    new void Start()
    {
        base.Start();
        tubeController = GetComponentInParent<ColorPuzzleController>();
        buttonPressSound = GetComponent<AudioSource>();
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
        buttonPressSound.Play();
        tubeController.SetTubeColor(tubeID);
    }
}
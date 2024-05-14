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
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2 && activate)
        {
            interactableImage.SetActive(true);
            interactableImage.transform.LookAt(Camera.main.transform.position);
            interactableImage.transform.rotation = new Quaternion(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z, -Camera.main.transform.rotation.w);
        }
        else interactableImage.SetActive(false);

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
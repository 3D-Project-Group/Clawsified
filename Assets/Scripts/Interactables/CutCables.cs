using UnityEngine;

public class CutCables : Interact
{
    private enum CableType { Camera, Door};
    [SerializeField] private CableType type;
    [SerializeField] private GameObject cameraToDeactivate;
    [SerializeField] private GameObject doorToDeactivate;

    public override void Interaction()
    {
        base.Interaction();
        if (type == CableType.Camera)
        {
            cameraToDeactivate.GetComponent<CameraAI>().activated = false;
        }
        else
        {
            doorToDeactivate.GetComponent<Door>().activated = false;
        }
        this.gameObject.SetActive(false);
    }
}

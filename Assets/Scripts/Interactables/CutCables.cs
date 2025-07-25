using UnityEngine;

public class CutCables : Interact
{
    private enum CableType { Camera, Door };

    [SerializeField] private CableType type;
    [SerializeField] private GameObject cameraToDeactivate;
    [SerializeField] private GameObject doorToDeactivate;
    [SerializeField] private AudioSource cutSound;

    public override void Interaction()
    {
        base.Interaction();
        cutSound.Play();
        if (type == CableType.Camera)
        {
            cameraToDeactivate.GetComponent<CameraAI>().activated = false;
        }
        else
        {
            doorToDeactivate.GetComponent<Animator>().SetBool("Open", true);
        }
    }
}
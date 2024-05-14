using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject interactableImage;
    public GameObject player;

    public bool activate = true;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public virtual void Interaction()
    {
        activate = false;
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2 && activate)
        {
            interactableImage.SetActive(true);
            interactableImage.transform.LookAt(Camera.main.transform.position);
        }
        else interactableImage.SetActive(false);
    }
}
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
            //Calculate the direction
            Vector3 direction = Camera.main.transform.position - interactableImage.transform.position;
            //Get the angle
            var rotation = Quaternion.LookRotation(direction);
            interactableImage.transform.rotation = Quaternion.Slerp(interactableImage.transform.rotation, rotation, 50);        }
        else interactableImage.SetActive(false);
    }
}
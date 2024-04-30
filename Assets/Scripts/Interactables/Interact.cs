using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject interactableImage;
    [SerializeField] private GameObject player;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public virtual void Interaction()
    {

    }
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 2)
        {
            interactableImage.SetActive(true);
            interactableImage.transform.LookAt(Camera.main.transform.position);
        }
        else interactableImage.SetActive(false);
    }
}

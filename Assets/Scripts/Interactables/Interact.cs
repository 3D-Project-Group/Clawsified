using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float interactRadius = 2;
    public Transform interactionCenter;
    public GameObject interactableImage;
    public GameObject player;

    public bool activate = true;

    public virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (interactionCenter == null)
            interactionCenter = this.transform;
    }
    public virtual void Interaction()
    {
        activate = false;
    }
    public virtual void Update()
    {
        if (Vector3.Distance(interactionCenter.position, player.transform.position) < interactRadius && activate)
        {
            interactableImage.SetActive(true);
        }
        else interactableImage.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(interactionCenter != null)
            Gizmos.DrawWireSphere(interactionCenter.position, interactRadius);
    }
}
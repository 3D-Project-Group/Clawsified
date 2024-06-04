using System;
using UnityEngine;

public class FinalBossShot : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private GameObject player;
    private PlayerController playerController;
    [SerializeField] private AudioSource poisonSound;
    [SerializeField] private GameObject poisonPuddle;
    
    [Header("Dmg Control")]
    [SerializeField] private float dmgRadius = 2f;
    [SerializeField] private float dmgMultiplier = 2f;
    [SerializeField] private float timeToDestroy = 3.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < dmgRadius)
        {
            playerController.TakeDamage(dmgMultiplier * Time.deltaTime);
        }
    }

    void PoisonSplash()
    {
        poisonPuddle.SetActive(true);
        this.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the collision point
        rb.drag = 100;
        this.transform.rotation = Quaternion.identity;
        
        PoisonSplash();
        
        poisonSound.Play();
        Destroy(this.gameObject, timeToDestroy);
    }
}
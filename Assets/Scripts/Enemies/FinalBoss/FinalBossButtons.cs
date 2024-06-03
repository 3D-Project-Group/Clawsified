using System.Collections;
using UnityEngine;

public class FinalBossButtons : Interact
{
    private FinalBoss boss;
    private bool activated = true;

    [Header("Explosion Control")]
    [SerializeField] private float timeToActivate = 5f;
    [SerializeField] private float damage = 20;
    [SerializeField] private int pipeSelected = 0;
    [SerializeField] private AudioSource explosionSound;

    [Space]
    [SerializeField] private GameObject[] pipesExplosionPoints;

    private new void Start()
    {
        base.Start();
        boss = GameObject.FindWithTag("Boss").GetComponent<FinalBoss>();
    }

    public override void Interaction()
    {
        base.Interaction();
        if (activated == true)
        {
            if (boss.currentPipe == pipeSelected)
            {
                if (boss.currentState == 0 || boss.currentState == 2) //If the boss is attacking or waiting
                {
                    boss.TakeDamage(damage);
                }
            }
            pipesExplosionPoints[pipeSelected].GetComponent<ParticleSystem>().Play();
            explosionSound.Play();
            activated = false;
            StartCoroutine(ActivateButton());
        }
    }

    IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(timeToActivate);
        activated = true;
    }
}
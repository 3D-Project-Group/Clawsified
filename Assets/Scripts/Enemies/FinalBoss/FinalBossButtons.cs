using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossButtons : Interact
{
    private FinalBoss boss;

    private bool activated = true;

    [SerializeField] private float timeToActivate = 5f;
    [SerializeField] private float damage = 20;

    [SerializeField] private int pipeSelected = 0;

    [SerializeField] private GameObject[] pipesExplosionPoints;

    private void Start()
    {
        boss = GameObject.FindWithTag("Boss").GetComponent<FinalBoss>();
    }

    public override void Interaction()
    {
        base.Interaction();
        if(activated == true)
        {
            if(boss.currentPipe == pipeSelected)
            {
                if (boss.currentState == 0 || boss.currentState == 2) //If the boss is attacking or waiting
                {
                    boss.TakeDamage(damage);
                }
            }
            pipesExplosionPoints[pipeSelected].GetComponent<ParticleSystem>().Play();
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

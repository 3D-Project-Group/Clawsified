using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { Normal, Speed, Blind }

    public EnemyType enemyType = EnemyType.Normal;

    [Header("Components")]
    public NavMeshAgent agent;
    public EnemyState currentState;
    public Transform player;
    public LayerMask obstructionMask, groundLayer;
    
    public Animator anim;
    [SerializeField] private AudioSource ratFootstepSound;
    [SerializeField] private AudioSource ratYellingSound;
    
    [Header("Enemy Calling")]
    [SerializeField] private float callRadius;
    [SerializeField] private LayerMask enemyLayer;
    [Space]
    public List<EnemyAI> enemiesList = new List<EnemyAI>();
    public List<EnemyAI> calledEnemiesList = new List<EnemyAI>();
    
    [Header("Attraction")]
    public bool beingAtracted = false;
    public float attractionTime = 3f;

    [Header("Patrol")]
    public List<GameObject> waypoints = new List<GameObject>();

    private bool invokedSound = false;

    void Start()
    {
        // Get waypoints
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            GameObject obj = transform.parent.GetChild(i).gameObject;
            if (obj != this.gameObject)
            {
                waypoints.Add(obj);
            }
        }
        waypoints.OrderBy(waypoint => waypoint.name).ToList(); //Organize it

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = new IdleState(gameObject, anim, agent, player, waypoints, obstructionMask, groundLayer);
        currentState.obstructionMask = obstructionMask;
        enemiesList = FindObjectsOfType<EnemyAI>().ToList();
    }

    void Update()
    {
        if (!beingAtracted)
        {
            currentState = currentState.Process();
            if (currentState.name == EnemyState.STATE.PURSUE)
            {
                if (!invokedSound)
                {
                    invokedSound = true;
                    Invoke("PlayYellingSound", Random.Range(1.5f, 2.5f));
                }
                CallOtherEnemies();
            }
            else
            {
                calledEnemiesList.Clear();
            }
        }
    }

    public IEnumerator Attract(Vector3 position)
    { 
        beingAtracted = true;
        agent.SetDestination(position);

        yield return new WaitForSeconds(attractionTime);
        
        beingAtracted = false;
        currentState = new RandomPatrolState(gameObject, anim, agent, player, waypoints, obstructionMask, groundLayer);
    }
    
    void PlayFootstepSound()
    {
        ratFootstepSound.Play();
    }
    
    void PlayYellingSound()
    {
        ratYellingSound.Play();
        invokedSound = false;
    }

    private void CallOtherEnemies()
    {
        foreach (EnemyAI enemy in enemiesList)
        {
            if (enemy.gameObject != this.gameObject && !calledEnemiesList.Contains(enemy) && Vector3.Distance(transform.position, enemy.gameObject.transform.position) <= callRadius)
            {
                calledEnemiesList.Add(enemy);
            }
        }
        if (calledEnemiesList.Count > 0)
        {
            foreach (EnemyAI enemy in calledEnemiesList)
            {
                enemy.calledEnemiesList = this.calledEnemiesList;
                if(enemy.currentState.name != EnemyState.STATE.PURSUE)
                    enemy.currentState = new PursueState(enemy.gameObject, enemy.anim, enemy.agent, enemy.player, enemy.waypoints, enemy.obstructionMask, enemy.groundLayer);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw Call Radius Sphere
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, callRadius);
    }
}
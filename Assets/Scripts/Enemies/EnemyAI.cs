using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { Normal, Speed, Blind }

    public EnemyType enemyType = EnemyType.Normal;

    [Header("Enemy Calling")]
    [SerializeField] private float callRadius;
    [SerializeField] private LayerMask enemyLayer;
    public List<EnemyAI> enemiesList = new List<EnemyAI>();
    public List<EnemyAI> calledEnemiesList = new List<EnemyAI>();

    public NavMeshAgent agent;
    public EnemyState currentState;

    public Transform player;
    public LayerMask obstructionMask, groundLayer;
    public bool beingAtracted = false;
    public float attractionTime = 3f;

    public List<GameObject> waypoints = new List<GameObject>();

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
        waypoints.OrderBy(waypoint => waypoint.name).ToList();

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = new IdleState(gameObject, agent, player, waypoints, obstructionMask, groundLayer);
        currentState.obstructionMask = obstructionMask;
        enemiesList = FindObjectsOfType<EnemyAI>().ToList();
    }

    void Update()
    {
        if (!beingAtracted)
        {
            currentState = currentState.Process();
            if (currentState.name == EnemyState.STATE.PURSUE)
                CallOtherEnemies();
            else
                calledEnemiesList.Clear();
        }
        else
        {
            Invoke("DeactivateAttraction", attractionTime);
        }
    }

    void DeactivateAttraction()
    {
        beingAtracted = false;
        currentState = new RandomPatrolState(gameObject, agent, player, waypoints, obstructionMask, groundLayer);
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
                enemy.currentState = new PursueState(enemy.gameObject, enemy.agent, enemy.player, enemy.waypoints, enemy.obstructionMask, enemy.groundLayer);
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
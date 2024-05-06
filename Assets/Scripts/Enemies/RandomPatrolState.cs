using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAI;

public class RandomPatrolState : EnemyState
{
    EnemyType enemyType;
    private int rotationSpeed = 5; 

    [Header("Walkpoint Control")]
    public Vector3 walkPoint;
    private bool walkPointSet = false;
    public float walkPointRange = 8;

    private float timer;

    public RandomPatrolState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.RANDOMPATROL;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
    }

    public override void Enter()
    {
        base.Enter();

        //Change the speed and viion distance according to the type of enemy
        switch (enemyType)
        {
            case EnemyType.Speed:
                agent.speed = 4;
                break;
            case EnemyType.Normal:
                agent.speed = 3;
                break;
            case EnemyType.Blind:
                agent.speed = 3;
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }

        agent.isStopped = false;

        timer = 10f;
    }

    public override void Update()
    {
        //If can see the player and there's nothing blocking the vision starts pursuing
        if (CanSeePlayer() && !Physics.Raycast(npc.transform.position, player.position, Vector3.Distance(npc.transform.position, player.transform.position), obstructionMask))
        {
            nextState = new PursueState(npc, agent, player, waypoints, obstructionMask, groundLayer);
            stage = EVENT.EXIT;
        }
        else if (timer > 0)
        {
            //Get a random point
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet && !agent.hasPath)
            {
                agent.SetDestination(walkPoint);

                Vector3 direction = walkPoint - npc.transform.position; //Calculate the direction
                direction.y = 0;

                var rotation = Quaternion.LookRotation(direction); //Gets the angle

                npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * rotationSpeed); //Rotate towards the point
            }

            float distanceToWalkPoint = Vector3.Distance(npc.transform.position, walkPoint); //Distance from the point

            // Walkpoint reached
            if (distanceToWalkPoint < agent.stoppingDistance)
            {
                agent.ResetPath();
                walkPointSet = false;
            }

            timer -= Time.deltaTime;
        }
        else
        {
            nextState = new PatrolState(npc, agent, player, waypoints, obstructionMask, groundLayer);
            stage = EVENT.EXIT;
        }
    }

    private void SearchWalkPoint()
    {
        agent.ResetPath();
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(npc.transform.position.x + randomX, npc.transform.position.y, npc.transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -npc.transform.up, 2f, groundLayer))
            walkPointSet = true;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
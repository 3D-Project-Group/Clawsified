using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolState : EnemyState
{
    [Header("Walkpoints")]
    public Vector3 walkPoint;
    private bool walkPointSet = false;
    public float walkPointRange = 8;
    EnemyAI.EnemyType enemyType;
    int damping = 2;

    float timer;

    public RandomPatrolState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.RANDOMPATROL;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
        switch (enemyType)
        {
            case EnemyAI.EnemyType.Speed:
                agent.speed = 4;
                break;
            case EnemyAI.EnemyType.Normal:
                agent.speed = 3;
                break;
            case EnemyAI.EnemyType.Blind:
                agent.speed = 3; 
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }
        agent.isStopped = false;
        timer = 10f;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer() && !Physics.Raycast(npc.transform.position, player.position, Vector3.Distance(npc.transform.position, player.transform.position), obstructionMask))
        {
            nextState = new PursueState(npc, agent, player, waypoints, obstructionMask, groundLayer);
            stage = EVENT.EXIT;
        }
        else if (timer > 0)
        {
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet && !agent.hasPath)
            {
                agent.ResetPath();
                agent.SetDestination(walkPoint);
                Vector3 direction = walkPoint - npc.transform.position;
                direction.y = 0;
                var rotation = Quaternion.LookRotation(direction);
                npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * damping);
            }

            float distanceToWalkPoint = Vector3.Distance(npc.transform.position, walkPoint);

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
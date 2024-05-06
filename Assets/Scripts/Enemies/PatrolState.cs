using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    int currentIndex = -1; // current waypoint
    int damping = 5;
    EnemyAI.EnemyType enemyType;
    float timeStarring = 0;
    float maxTimeStarring = 3;
    public PatrolState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.PATROL;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
        switch (enemyType)
        {
            case EnemyAI.EnemyType.Speed:
                agent.speed = 3;
                break;
            case EnemyAI.EnemyType.Normal:
                agent.speed = 2;
                break;
            case EnemyAI.EnemyType.Blind:
                agent.speed = 2;
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }
        agent.isStopped = false;
    }

    public override void Enter()
    {
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= waypoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.ResetPath();
            agent.SetDestination(waypoints.ElementAt(currentIndex).transform.position);
            Vector3 direction = waypoints.ElementAt(currentIndex).transform.position - npc.transform.position;

            var rotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * damping);
        }
        if (CanSeePlayer() && !Physics.Raycast(npc.transform.position, player.position, Vector3.Distance(npc.transform.position, player.transform.position), obstructionMask))
        {
            agent.ResetPath();
            Vector3 direction = player.position - npc.transform.position;
            var rotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * damping);

            timeStarring += Time.deltaTime + (0.1f / Vector3.Distance(npc.transform.position, player.position));
            if(timeStarring >= maxTimeStarring)
            {
                nextState = new PursueState(npc, agent, player, waypoints, obstructionMask, groundLayer);
                stage = EVENT.EXIT;
            }
        }else if (timeStarring > 0)
        {
            timeStarring -= Time.deltaTime;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
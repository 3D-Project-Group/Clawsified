using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PursueState : EnemyState
{
    int damping = 2;
    float currentStamina = 5f;
    float maxStamina = 5f;
    EnemyAI.EnemyType enemyType;
    bool isResting = false;

    public PursueState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.PURSUE;
        agent.isStopped = false;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
        switch (enemyType)
        {
            case EnemyAI.EnemyType.Speed:
                agent.speed = 5;
                break;
            case EnemyAI.EnemyType.Normal:
                agent.speed = 4;
                break;
            case EnemyAI.EnemyType.Blind:
                agent.speed = 4; 
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        

        if (agent.hasPath)
        {
            if (Vector3.Distance(npc.transform.position, player.position) <= agent.stoppingDistance)
            {
                player.gameObject.GetComponent<PlayerController>().Death();
            }
            else
            {
                if (!CanSeePlayer())
                {
                    nextState = new RandomPatrolState(npc, agent, player, waypoints, obstructionMask, groundLayer);
                    stage = EVENT.EXIT;
                }
                else
                {
                    if (enemyType == EnemyAI.EnemyType.Speed)
                    {
                        if (currentStamina > 0 && !isResting)
                        {
                            currentStamina -= Time.deltaTime;
                            agent.speed = 5;
                        }
                        else if (currentStamina <= 0 || isResting)
                        {
                            isResting = true;
                            agent.speed = 0;
                            currentStamina += Time.deltaTime;
                            if (currentStamina >= maxStamina)
                            {
                                isResting = false;
                            }
                        }
                    }
                    Vector3 direction = player.position - npc.transform.position;
                    direction.y = 0;
                    var rotation = Quaternion.LookRotation(direction);
                    npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * damping);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
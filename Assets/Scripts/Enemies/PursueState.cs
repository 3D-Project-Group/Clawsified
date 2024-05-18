using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAI;

public class PursueState : EnemyState
{
    EnemyType enemyType;

    private int rotationSpeed = 10;
    private float minDistanceOfPlayer = 5;

    [Header("Stamina Control")]
    private float currentStamina = 5f;
    private float maxStamina = 5f;
    private bool isResting = false;

    public PursueState(GameObject _npc, Animator _anim, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _anim, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.PURSUE;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
        
    }

    public override void Enter()
    {
        base.Enter();

        //Change speed and vision cone according to the type of enemy
        switch (enemyType)
        {
            case EnemyType.Speed:
                agent.speed = 5;
                break;
            case EnemyType.Normal:
                agent.speed = 4;
                break;
            case EnemyType.Blind:
                agent.speed = 4;
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }

        agent.isStopped = false;
    }

    public override void Update()
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Running", true);
        
        agent.SetDestination(player.position);

        if (agent.hasPath)
        {
            //If the rat gets close to the player
            if (Vector3.Distance(npc.transform.position, player.position) <= 2)
            {
                player.gameObject.GetComponent<PlayerController>().Death();
            }
            else
            {
                //If can't see the player starts Random patrolling
                if (!CanSeePlayer() && Vector3.Distance(npc.transform.position, player.position) > minDistanceOfPlayer)
                {
                    nextState = new RandomPatrolState(npc, anim, agent, player, waypoints, obstructionMask, groundLayer);
                    stage = EVENT.EXIT;
                }
                else
                {
                    //If is a speed enemy, starts to decrease the stamina
                    if (enemyType == EnemyType.Speed)
                    {
                        if (currentStamina > 0 && !isResting)
                        {
                            currentStamina -= Time.deltaTime;
                            agent.speed = 5;
                        }
                        //Rest
                        else if (currentStamina <= 0 || isResting)
                        {
                            isResting = true;
                            agent.speed = 0;
                            currentStamina += Time.deltaTime;
                            if (currentStamina >= maxStamina)
                            {
                                isResting = false; //Reset resting
                            }
                        }
                    }
                    Vector3 direction = player.position - npc.transform.position; //Calculate Direction
                    direction.y = 0;

                    var rotation = Quaternion.LookRotation(direction); //Gets the angle

                    npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * rotationSpeed); //Rotate towards the player
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
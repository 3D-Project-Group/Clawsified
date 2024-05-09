using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static EnemyAI;

public class PatrolState : EnemyState
{
    EnemyType enemyType;

    private int currentIndex = -1; // current waypoint
    private int rotationSpeed = 5;

    [Header("Staring")]
    private float timeStaring = 0;
    private float maxTimeStaring = 3;

    public PatrolState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.PATROL;
        enemyType = npc.GetComponent<EnemyAI>().enemyType;
    }

    public override void Enter()
    {
        base.Enter();

        //Change the speed and the vision according to the type of enemy
        switch (enemyType)
        {
            case EnemyType.Speed:
                agent.speed = 3;
                break;
            case EnemyType.Normal:
                agent.speed = 2;
                break;
            case EnemyType.Blind:
                agent.speed = 2;
                visDist = visDist / 1.3f;
                break;
            default:
                break;
        }
        agent.isStopped = false;

        currentIndex = 0;
    }

    public override void Update()
    {
        //If can see the player and there's nothing blocking the vision
        if (CanSeePlayer() && !Physics.Raycast(npc.transform.position, player.position, Vector3.Distance(npc.transform.position, player.transform.position), obstructionMask))
        {
            //Reset the path, then we can set a new direction for it
            agent.ResetPath();

            Vector3 direction = player.position - npc.transform.position; //Calculate the direction
            var rotation = Quaternion.LookRotation(direction); //Get the angle
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * rotationSpeed); //Rotate towards player

            //Keeps staring for a moment
            timeStaring += Time.deltaTime + (0.1f / Vector3.Distance(npc.transform.position, player.position));

            if(timeStaring >= maxTimeStaring)
            {
                nextState = new PursueState(npc, agent, player, waypoints, obstructionMask, groundLayer);
                stage = EVENT.EXIT;
            }
        }
        else if (timeStaring > 0)
        {
            timeStaring -= Time.deltaTime;
        }
        else
        {
            if (agent.remainingDistance < 1)
            {
                //Update waypoint
                if (currentIndex >= waypoints.Count - 1)
                    currentIndex = 0;
                else
                    currentIndex++;

                //Set the direction to the next waypoint
                agent.ResetPath();
                agent.SetDestination(waypoints.ElementAt(currentIndex).transform.position);

                //Calculate the direction
                Vector3 direction = waypoints.ElementAt(currentIndex).transform.position - npc.transform.position;
                //Get the angle
                var rotation = Quaternion.LookRotation(direction);
                //Rotate towards next waypoint
                npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyState
{
    public IdleState(GameObject _npc,Animator _anim, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
        : base(_npc, _anim, _agent, _player, _waypoints, _obstructionMask, _groundLayer)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (Random.Range(0, 100) < 10)
        {
            nextState = new PatrolState(npc, anim, agent, player, waypoints, obstructionMask, groundLayer);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

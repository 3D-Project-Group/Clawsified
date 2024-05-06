using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState
{
    public enum STATE
    {
        IDLE,
        PATROL,
        PURSUE,
        RANDOMPATROL
    };
    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    };

    public STATE name;

    [Header("Components")]
    protected EVENT stage;
    protected GameObject npc;
    protected Transform player;
    protected EnemyState nextState;
    protected NavMeshAgent agent;
    public PlayerController playerController;

    [Header("Pursue")]
    public float visDist = 10.0f;
    public float visAngle = 60.0f;
    public LayerMask obstructionMask, groundLayer;

    [Header("Patrol")]
    public List<GameObject> waypoints = new List<GameObject>();

    public EnemyState(GameObject _npc, NavMeshAgent _agent, Transform _player, List<GameObject> _waypoints, LayerMask _obstructionMask, LayerMask _groundLayer)
    {
        npc = _npc;
        agent = _agent;
        player = _player;
        waypoints = _waypoints;
        obstructionMask = _obstructionMask;
        groundLayer = _groundLayer;
        playerController = player.gameObject.GetComponent<PlayerController>();
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public EnemyState Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle && !playerController.isHidden)
        {
            return true;
        }
        return false;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CameraAI : MonoBehaviour
{
    Material objMaterial;
    LineRenderer lineRenderer;

    public Transform player;
    public PlayerController playerController;
    public bool activated = true;

    [Header("Enemy Calling")]
    [SerializeField] private float callRadius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private List<EnemyAI> enemiesList = new List<EnemyAI>();
    [SerializeField] private List<EnemyAI> calledEnemiesList = new List<EnemyAI>();
    [Space]
    public float visDist = 20.0f;
    public float visAngle = 30.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        enemiesList = FindObjectsOfType<EnemyAI>().ToList();
        objMaterial = this.gameObject.GetComponent<MeshRenderer>().material;
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        if (activated)
        {
            if (!CanSeePlayer())
            {
                calledEnemiesList.Clear();
                objMaterial.color = Color.green;
            }
            else
            {
                CallOtherEnemies();
                objMaterial.color = Color.red;
            }
        }
        else
        {
            objMaterial.color = Color.black;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }
    }

    private void CallOtherEnemies()
    {
        foreach (EnemyAI enemy in enemiesList)
        {
            if (enemy.gameObject != this.gameObject && !calledEnemiesList.Contains(enemy) 
                    && Vector3.Distance(transform.position, enemy.gameObject.transform.position) <= callRadius)
            {
                calledEnemiesList.Add(enemy);
            }
        }
        if (calledEnemiesList.Count > 0)
        {
            foreach (EnemyAI enemy in calledEnemiesList)
            {
                enemy.calledEnemiesList = this.calledEnemiesList;
                enemy.currentState = new PursueState(enemy.gameObject, enemy.anim, enemy.agent, enemy.player, enemy.waypoints, enemy.obstructionMask, enemy.groundLayer);
            }
        }
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (direction.magnitude < visDist && angle < visAngle && !playerController.isInvisible)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //----------->This was made to see the camera vision cone, but we can't build the game with that, so we commented, just undo if you wanna see it :D<-----------

        //Vector3 forwardDir = transform.forward;
        //Vector3 leftDir = Quaternion.Euler(0, -visAngle / 2, 0) * forwardDir;
        //Vector3 rightDir = Quaternion.Euler(0, visAngle / 2, 0) * forwardDir;

        // Draw angle lines
        //Handles.color = Color.blue;
        //Handles.DrawLine(transform.position, transform.position + leftDir * visDist);
        //Handles.DrawLine(transform.position, transform.position + rightDir * visDist);

        // Draw arc
        //Handles.color = Color.blue;
        //Handles.DrawWireArc(transform.position, Vector3.up, leftDir, visAngle, visDist);

        // Draw Call Radius Sphere
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, callRadius);
    }
}
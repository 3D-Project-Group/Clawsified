using UnityEngine;

public class DroppableObjects : Interact
{
    private enum ThrowDirection { LEFT, RIGHT, FRONT, BACK }

    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private ThrowDirection direction;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Throw Control")]
    [SerializeField] private float throwingForce;
    [SerializeField] private float callRadius;

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public override void Interaction()
    {
        if (activate)
        {
            base.Interaction();
            //Add force according to the direction chosen
            switch (direction)
            {
                case ThrowDirection.LEFT:
                    rb.AddForce(this.transform.position + Camera.main.gameObject.transform.right * -1 * throwingForce);
                    break;
                case ThrowDirection.RIGHT:
                    rb.AddForce(this.transform.position + Camera.main.gameObject.transform.right * throwingForce);
                    break;
                case ThrowDirection.FRONT:
                    rb.AddForce(this.transform.position + Camera.main.gameObject.transform.forward * throwingForce);
                    break;
                case ThrowDirection.BACK:
                    rb.AddForce(this.transform.position + Camera.main.gameObject.transform.forward * -1 * throwingForce);
                    break;
                default:
                    Debug.Log("Set a direction to the object!");
                    break;
            }

            //Attract Enemies
            Collider[] colliders = Physics.OverlapSphere(transform.position, callRadius, enemyLayer);

            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    print("Called");
                    EnemyAI enemy = collider.gameObject.GetComponent<EnemyAI>();
                    if (enemy.beingAtracted == false)
                    {
                        enemy.agent.SetDestination(transform.position);
                        enemy.beingAtracted = true;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, callRadius);
    }
}
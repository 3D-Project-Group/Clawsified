using UnityEngine;
using UnityEngine.AI;

public class CheeseController : MonoBehaviour
{
    [SerializeField] private float atractEnemiesRadius = 1f;
    [SerializeField] private LayerMask enemiesLayer;
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, atractEnemiesRadius, enemiesLayer);

        // If it finds any object, calls the method "Interaction" inside the object
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                if (collider.gameObject.GetComponent<EnemyAI>().beingAtracted == false)
                {
                    collider.gameObject.GetComponent<EnemyAI>().beingAtracted = true;
                }
            }
        }
    }
}

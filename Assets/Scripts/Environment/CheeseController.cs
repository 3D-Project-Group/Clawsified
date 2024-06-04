using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class CheeseController : MonoBehaviour
{
    [SerializeField] private float attractEnemiesRadius = 1f;
    [SerializeField] private LayerMask enemiesLayer;
    private bool attracted = false;
    void Update()
    {
        if (!attracted)
        {
            Attract();
            Invoke("ResetAttraction", 1);
        }
    }

    void ResetAttraction()
    {
        attracted = false;
    }

    void Attract()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractEnemiesRadius, enemiesLayer);
        
        // If it finds any object, calls the method "Interaction" inside the object
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                EnemyAI enemy = collider.gameObject.GetComponent<EnemyAI>();
                if (enemy.beingAtracted == false)
                {
                    StartCoroutine(enemy.Attract(transform.position));
                }
            }
        }

        attracted = true;
    }
}

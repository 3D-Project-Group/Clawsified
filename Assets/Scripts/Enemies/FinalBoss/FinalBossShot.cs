using UnityEngine;

public class FinalBossShot : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private GameObject player;
    private PlayerController playerController;
    private Vector3 collisionPosition;

    [Header("Dmg Control")]
    [SerializeField] private float timeToDestroy = 3.0f;
    [SerializeField] private float dmgRadius = 2f;
    [SerializeField] private float dmgMultiplier = 2f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Draw")]
    [SerializeField] private int circleSegments = 32;
    [SerializeField] private Material lineMaterial;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        lineRenderer = (LineRenderer)gameObject.AddComponent(typeof(LineRenderer));

        lineRenderer.material = lineMaterial; // Assign the material for the lines
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = circleSegments + 1; // +1 to close the circle
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < dmgRadius)
        {
            playerController.TakeDamage(dmgMultiplier * Time.deltaTime);
        }
    }

    void DrawCircle()
    {
        // Calculate points for the circle
        for (int i = 0; i < circleSegments; i++)
        {
            float angle = (i * 2f * Mathf.PI) / circleSegments;
            float x = Mathf.Cos(angle) * dmgRadius;
            float z = Mathf.Sin(angle) * dmgRadius;
            Vector3 point = collisionPosition + new Vector3(x, 0f, z);
            lineRenderer.SetPosition(i, point);
        }

        // Close the circle
        lineRenderer.SetPosition(circleSegments, lineRenderer.GetPosition(0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the collision point
        rb.drag = 100;
        collisionPosition = collision.contacts[0].point;
        collisionPosition.y = 0f; // Ensure the circle is drawn on the ground
        DrawCircle();
        Destroy(this.gameObject, timeToDestroy);
    }
}
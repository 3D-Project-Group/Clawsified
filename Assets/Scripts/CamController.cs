using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform target;
    [SerializeField] private PlayerController player;

    [Header("Cam Movement")]
    public bool canMoveCam = true;
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float currentDistance = 8f;
    [SerializeField] private LayerMask layersToCollide;

    bool isColliding = true;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (canMoveCam)
        {
            transform.RotateAround(target.position, transform.up, Input.GetAxis("Mouse X") * sensitivityX);
            transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * sensitivityY);

            Vector3 rotation = transform.eulerAngles;
            rotation.z = 0;
            if (rotation.x < 180) rotation.x = Mathf.Min(rotation.x, 80);
            else rotation.x = 0;
            transform.rotation = Quaternion.Euler(rotation);

            transform.position = target.position - transform.forward * currentDistance;

            CheckCollision();
            ScrollControl();
        }

        if (player.isHidden || player.doingPuzzle || Time.timeScale == 0)
            canMoveCam = false;
        else
            canMoveCam = true;
    }

    void CheckCollision()
    {
        if (Physics.Linecast(target.position, target.position - this.transform.forward * maxDistance, out RaycastHit hit))
        {
            if ((layersToCollide & 1 << hit.collider.gameObject.layer) == 1 << hit.collider.gameObject.layer)
            {
                this.transform.position = hit.point + this.transform.forward * 2.0f;
                isColliding = true;
            }
        }
        else if (isColliding)
        {
            this.transform.position = target.position - this.transform.forward * currentDistance;
            isColliding = false;
        }
    }

    void ScrollControl()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentDistance > minDistance)
            {
                currentDistance -= 0.5f;
                if (!isColliding)
                    this.transform.position = target.position - this.transform.forward * currentDistance;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentDistance < maxDistance)
            {
                currentDistance += 0.5f;
                if (!isColliding)
                    this.transform.position = target.position - this.transform.forward * currentDistance;
            }
        }
    }
}
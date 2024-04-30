using System.Collections;
using System.Collections.Generic;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.Find("Cat").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (canMoveCam)
        {
            transform.RotateAround(target.position, transform.up, Input.GetAxis("Mouse X") * sensitivityX);
            transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * sensitivityY);

            Vector3 rotacao = transform.eulerAngles;
            rotacao.z = 0;
            if (rotacao.x < 180) rotacao.x = Mathf.Min(rotacao.x, 80);
            else rotacao.x = 0;
            transform.rotation = Quaternion.Euler(rotacao);

            transform.position = target.position - transform.forward * currentDistance;

            CheckCollision();
            ScrollControl();
        }

        if (player.isHidden)
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
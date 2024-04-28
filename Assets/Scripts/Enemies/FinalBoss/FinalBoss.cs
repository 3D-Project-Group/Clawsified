using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] private int lastState = -1;
    public int currentState = -1; //0 = Waiting, 1 = Changing Tube, 2 = Attacking

    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private Transform[] tubeSpawnPoint;
    [SerializeField] private Transform[] tubeEndingPoint;
    public int currentPipe;
    [SerializeField] private float waitTimeToChangeTube;

    [Header("Stats")]
    [SerializeField] private float bossMaxHp;
    [SerializeField] private float bossCurrentHp;
    [SerializeField] private float speed;

    [Header("States")]
    [SerializeField] private bool isWaiting = false;
    [SerializeField] private bool isSwitching = false;
        [SerializeField] private bool goingIn = false;
        [SerializeField] private bool goingOut = false;
    [SerializeField] private bool isAttacking = false;

    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>(); 

        currentPipe = Random.Range(0, tubeSpawnPoint.Length);
        currentState = 1;
        transform.position = tubeSpawnPoint[currentPipe].position;
        bossCurrentHp = bossMaxHp;
    }

    void Update()
    {

        switch (currentState)
        {
            case 0: 
                if(!isWaiting)
                    StartCoroutine(Wait());
                break;
            case 1:
                if (isSwitching)
                {
                    if (goingIn)
                    {
                        Vector3 direction = tubeSpawnPoint[currentPipe].position - transform.position;
                        direction.y = 0;

                        var targetRotation = Quaternion.LookRotation(direction);

                        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                        {
                            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

                            if (Vector3.Distance(transform.position, tubeSpawnPoint[currentPipe].position) < 2)
                            {
                                goingIn = false;
                                SwitchTube();
                            }
                        }
                        else
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                        }
                    }
                    else if (goingOut)
                    {
                        Vector3 direction = tubeEndingPoint[currentPipe].position - transform.position;
                        direction.y = 0;

                        var targetRotation = Quaternion.LookRotation(direction);

                        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                        {
                            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

                            if (Vector3.Distance(transform.position, tubeEndingPoint[currentPipe].position) < 1)
                            {
                                isSwitching = false;
                                goingOut = false;
                                currentState = -1;
                            }
                        }
                        else
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                        }
                    }
                }
                else
                {
                    GoInside();
                }
                break;
            case 2:
                if(!isAttacking)
                    Attack();
                break;
            default:
                int i = Random.Range(0, 3);
                if (i != lastState)
                {
                    currentState = i;
                    lastState = currentState;
                }
                break;
        }
    }

    void GoInside()
    {
        currentState = 1;
        isSwitching = true;
        goingIn = true;
    }

    void GoOutside()
    {
        goingOut = true;
    }

    void SwitchTube()
    {
        currentPipe = Random.Range(0, tubeSpawnPoint.Length);
        transform.position = tubeSpawnPoint[currentPipe].position;
        GoOutside();
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        currentState = 0;
        yield return new WaitForSeconds(waitTimeToChangeTube);
        isWaiting = false;
        currentState = -1;
    }

    void Attack()
    {
        isAttacking = true;
        currentState = 2;

        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        ApplyForceOnProj(rb);

        currentState = 0;
        isAttacking = false;
    }

    void ApplyForceOnProj(Rigidbody rb)
    {
        //float projForce = Mathf.Sqrt(2 * rb.mass * Physics.gravity.magnitude * (player.transform.position - transform.position).magnitude);

        float distance = Vector3.Distance(player.transform.position, transform.position);
        float time = Mathf.Sqrt(2 * distance / Physics.gravity.magnitude);
        float horizontalSpeed = distance / time;
        float projHeight = Mathf.Abs(player.transform.position.y - transform.position.y) + 1.0f; // Jump Height
        float verticalSpeed = Mathf.Sqrt(2 * Physics.gravity.magnitude * projHeight);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 horizontalVelocity = direction * horizontalSpeed;
        Vector3 verticalVelocity = Vector3.up * verticalSpeed;

        rb.velocity = horizontalVelocity + verticalVelocity;
    }

    public void TakeDamage(float amount)
    {
        bossCurrentHp -= amount;
        if(bossCurrentHp <= 0)
        {
            //Call Win Scene
        }
    }
}

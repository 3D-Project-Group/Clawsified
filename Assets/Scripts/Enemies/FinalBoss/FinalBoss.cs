using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float bossCurrentHp;
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
        bossCurrentHp = bossMaxHp;

        //Set a tube for the boss
        currentPipe = Random.Range(0, tubeSpawnPoint.Length);
        currentState = 1;
        transform.position = tubeSpawnPoint[currentPipe].position;
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
                        //Calculate the direction
                        Vector3 direction = tubeSpawnPoint[currentPipe].position - transform.position;
                        direction.y = 0;

                        //Get the angle for the direction
                        var targetRotation = Quaternion.LookRotation(direction);

                        //If the boss is looking to the right direction it starts walking, otherwise it keeps rotating
                        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                        {
                            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

                            //If its close enough it stops
                            if (Vector3.Distance(transform.position, tubeSpawnPoint[currentPipe].position) < 2)
                            {
                                goingIn = false;
                                SwitchTube();
                            }
                        }
                        else
                        {
                            //Rotate the boss
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                        }
                    }
                    else if (goingOut)
                    {
                        //Calculate the direction
                        Vector3 direction = tubeEndingPoint[currentPipe].position - transform.position;
                        direction.y = 0;

                        //Get the angle for the direction
                        var targetRotation = Quaternion.LookRotation(direction);

                        //If the boss is looking to the right direction it starts walking, otherwise it keeps rotating
                        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                        {
                            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

                            //If its close enough it stops
                            if (Vector3.Distance(transform.position, tubeEndingPoint[currentPipe].position) < 1)
                            {
                                isSwitching = false;
                                goingOut = false;
                                currentState = -1;
                            }
                        }
                        else
                        {
                            //Rotate the boss
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
                //Randomize a new state
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

        //Calculate player's direction
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.y = 0;
        //Get the angle
        Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

        //Rotate the boss
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        //Instantiate the projectile and throw it
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        ApplyForceOnProj(rb);

        currentState = 0;
        isAttacking = false;
    }

    void ApplyForceOnProj(Rigidbody rb)
    {
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
            SceneManager.LoadSceneAsync("WinMenu");
        }
    }
}

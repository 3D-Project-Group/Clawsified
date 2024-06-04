using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject UICam;
    [Space]
    //Ground Check
    [SerializeField] private Transform groundCheckStart;
    [SerializeField] private Transform groundCheckEnd;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [Space]
    //Animations
    [SerializeField] private Animator anim;
    [SerializeField] private Animator transitionAnimator;
    [Space]
    //Sounds
    [SerializeField] private AudioSource deathSound;
    [Space]
    //Testing
    [SerializeField] private TMP_Text fpsText;
    
    [Header("Public Bools")]
    public bool isMeowing;
    public bool isDead;
    public bool isHidden = false;
    public bool canHide = false;
    public bool canJump = false;
    public bool canWalk = false;
    public bool isResting = false;
    public bool isRunning = false;
    public bool doingPuzzle = false;

    [Header("Player Stats")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina = 100f;
    [SerializeField] private float maxHp = 100f;
    [SerializeField] private float currentHp = 100f;
    [SerializeField] private float playerRestoreHpTime = 4f;

    [Header("Player Movement")]
    [Range(1f, 10f)]
    [SerializeField] private float staminaLossMultiplier = 3f;

    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float rotationSpeed = 2f;

    [Header("Throw Cheese")]
    [SerializeField] private GameObject cheesePrefab;
    [SerializeField] private float amountOfCheese = 2f;
    [SerializeField] private float cheeseCooldown;
    private float cheeseCooldownCurrentValue;

    [Space]
    [SerializeField] private float projectileSpeed = 10.0f;
    [SerializeField] private float projectileDistance = 2.0f;
    [SerializeField] private float projectileAngle = 45.0f;
    private bool canThrowCheese = true;

    [Header("Hiding")]
    [SerializeField] private Transform camGoalPosition;
    [SerializeField] private float camLerpVelocity;
    private SkinnedMeshRenderer playerRenderer;

    [Header("Interact")]
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask interactLayer;

    [Header("Player UI")]
    [SerializeField] private Slider staminaWheel;
    [SerializeField] private Animator staminaWheelAnim;
    [Space]
    [CanBeNull][SerializeField] private GameObject jumpUI;
    [Space]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text cheeseText;
    [SerializeField] private Slider cheeseCooldownSlider;
    [SerializeField] private Slider playerHpSlider; //Just for boss fight

    [Header("Cheats")]
    [SerializeField] private GameObject cheatList;

    [Header("Player Meow :3")]
    [SerializeField] private AudioSource meowSource;
    [SerializeField] private AudioClip[] meowSounds;
    [SerializeField] private float attractEnemiesRadius;
    [SerializeField] private LayerMask enemiesLayer;
    
    public bool isInvisible;

    /*Jump Variables*/
    private Transform[] jumpWaypoints = new Transform[8];
    private Vector3 jumpGoal;
    private Transform jumpUIPosition;
    
    private Vector3 defaultGravity = new Vector3(0, -9.81f, 0);
    private Vector3 jumpGravity = new Vector3(0, -38f, 0);
    
    private bool isJumping = false;
    
    /*Movement  Variables*/
    private Vector3 movementDirection;
    private float hor;
    private float ver;

    private float lastTimeTookDmg;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        staminaWheel.maxValue = maxStamina;
        cheeseCooldownSlider.maxValue = cheeseCooldown; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(fpsText != null)
            fpsText.text = ((int)(1 / Time.deltaTime)).ToString();
        if (!doingPuzzle)
        {
            
            if(cheatList != null && Input.GetKeyDown(KeyCode.J))
            {
                if (cheatList.activeSelf)
                {
                    cheatList.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    cheatList.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeSelf)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gameManager.PauseGameSounds();
                pauseMenu.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F) && canHide && !isHidden && !isMeowing)
                StartCoroutine(Hide());
            else if (Input.GetKeyDown(KeyCode.F) && isHidden)
                UnHide();
            else if (Input.GetKeyDown(KeyCode.F) && !isMeowing)
                CallInteraction();

            if (!isHidden) 
            {
                if (Input.GetMouseButtonDown(0) && canWalk && !isHidden && !isMeowing)
                {
                    StartCoroutine(MeowAction());
                }
                
                if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isResting && !isMeowing)
                {
                    if (hor != 0 || ver != 0)
                    {
                        isRunning = true;
                        currentSpeed = runningSpeed;
                        currentStamina -= staminaLossMultiplier * Time.deltaTime;
                    }
                    else
                    {
                        isRunning = false;
                    }
                }
                else
                {
                    isRunning = false;
                    currentSpeed = normalSpeed;
                }
                anim.SetFloat("currentSpeed", currentSpeed);

                if (Input.GetKeyDown(KeyCode.Space) && canJump && !isJumping && !isMeowing)
                {
                    Transform closestWaypoint = null;
                    foreach (Transform t in jumpWaypoints)
                    {
                        if (t != null)
                        {
                            if (closestWaypoint == null)
                                closestWaypoint = t;
                            else if (Vector3.Distance(transform.position, t.position) < Vector3.Distance(transform.position, closestWaypoint.position))
                                closestWaypoint = t;
                        }
                    }
                    Jump(new Vector3(closestWaypoint.position.x, closestWaypoint.position.y, closestWaypoint.position.z));
                }

                if (Input.GetMouseButtonDown(1) && amountOfCheese > 0 && canThrowCheese)
                    ThrowCheese();

                if (!Physics.CheckCapsule(groundCheckStart.position, groundCheckEnd.position, groundCheckRadius, groundLayer) && !isJumping && rb.velocity.y < 0.7f)
                {
                    anim.SetBool("isFalling", true);
                }
                else
                {
                    anim.SetBool("isFalling", false);
                }
                    
                canWalk = Physics.CheckCapsule(groundCheckStart.position, groundCheckEnd.position, groundCheckRadius, groundLayer) && !isJumping && !isMeowing;
                if(canWalk && Physics.gravity != defaultGravity)
                    Physics.gravity = defaultGravity;
            }
            else
            {
                isRunning = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (cheeseCooldownCurrentValue > 0)
            cheeseCooldownCurrentValue -= Time.fixedDeltaTime;
        if (!doingPuzzle)
        {
            staminaWheel.value = currentStamina;
            if (currentStamina <= 0)
            {
                isResting = true;
                isRunning = false;
            }

            if (isResting && currentStamina >= 20)
                isResting = false;

            if (currentStamina < maxStamina && !isRunning)
                currentStamina += 0.1f;

            if (isJumping && Physics.CheckCapsule(groundCheckStart.position, groundCheckEnd.position, groundCheckRadius, groundLayer) && anim.GetBool("isJumping") == false)
            {
                isJumping = false;
                anim.applyRootMotion = true;
            }

            if (isHidden && UICam.activeSelf)
                UICam.SetActive(false);
            else if(!isHidden && !UICam.activeSelf)
                UICam.SetActive(true);

            if (canWalk)
                Movement();

            if (Time.time - lastTimeTookDmg >= playerRestoreHpTime && currentHp < maxHp)
                currentHp += Time.fixedDeltaTime / 2;

            UIControl();
        }
    }

    void UIControl()
    {
        cheeseCooldownSlider.value = cheeseCooldownCurrentValue;
        cheeseText.text = amountOfCheese.ToString();

        if(playerHpSlider != null)
        {
            playerHpSlider.value = currentHp;
        }

        if (isRunning)
        {
            staminaWheelAnim.ResetTrigger("FadeOut");
            staminaWheelAnim.SetTrigger("FadeIn");
        }
        else
        {
            staminaWheelAnim.ResetTrigger("FadeIn");
            staminaWheelAnim.SetTrigger("FadeOut");
        }

        if (canJump)
        {
            jumpUI.SetActive(true);
        }
        else if(jumpUI != null)
        {
            jumpUI.SetActive(false);
        }
    }

    IEnumerator MeowAction()
    {
        isMeowing = true;
        anim.SetBool("isMoving", false);
        AudioClip audio = meowSounds[Random.Range(0, meowSounds.Length)];
        meowSource.PlayOneShot(audio);
        
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
                    yield return null;
                }
            }
        }
        yield return new WaitForSeconds(audio.length);

        isMeowing = false;
    }

    public void AddCheese(int amount)
    {
        amountOfCheese += amount;
    }

    void ActivateCheese()
    {
        canThrowCheese = true;
    }

    private void CallInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRadius, interactLayer);

        // If it finds any object, calls the method "Interaction" inside the object
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<Interact>().Interaction();
            }
        }
    }

    private void ThrowCheese()
    {
        amountOfCheese--;
        cheeseCooldownCurrentValue = cheeseCooldown;
        canThrowCheese = false;
        // Instantiate the prefab
        GameObject projectile = Instantiate(cheesePrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);

        // Calculate the parabolic trajectory
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0;
        direction.Normalize();
        Vector3 gravity = Physics.gravity * projectileDistance;
        float y = projectileDistance * Mathf.Tan(projectileAngle * Mathf.Deg2Rad) + 0.5f * gravity.y * Mathf.Pow(projectileDistance / projectileSpeed, 2f);

        // Add velocity to the projectile
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed + Vector3.up * y;

        Invoke("ActivateCheese", cheeseCooldown);
        Destroy(projectile, 5f);
    }

    private IEnumerator Hide()
    {
        isHidden = true;
        
        //Creating variable to avoid errors by missing the position when we move the player
        Vector3 goalPosition = camGoalPosition.position;
        Quaternion goalRotation = camGoalPosition.rotation;
        
        playerRenderer.enabled = false;
        transform.rotation = goalRotation;
        transform.position = goalPosition;

        float t = 0;
        float time = 0.25f;
        for (; t < time; t += camLerpVelocity * Time.deltaTime)
        {
            Camera.main.gameObject.transform.position = Vector3.Slerp(Camera.main.gameObject.transform.position, goalPosition, t / time);
            Camera.main.gameObject.transform.rotation = goalRotation;
            yield return null;
        }
    }

    private void UnHide()
    {
        isHidden = false;
        playerRenderer.enabled = true;
    }

    private void Jump(Vector3 goal)
    {
        isJumping = true;
        jumpGoal = goal;
        transform.LookAt(new Vector3(jumpGoal.x, transform.position.y, jumpGoal.z));

        anim.applyRootMotion = false;
        
        anim.SetBool("isJumping", true);
    }

    private void ApplyJumpForce()
    {
        Vector3 direction = jumpGoal - transform.position;
        Vector3 newDirection = new Vector3(direction.x / 2, direction.y * 2, direction.z / 2 );
        newDirection.Normalize();

        // Scale direction vector by force magnitude
        Physics.gravity = jumpGravity;
        Vector3 forceVector = newDirection * Mathf.Sqrt(2 * rb.mass * Physics.gravity.magnitude * (jumpGoal - transform.position).magnitude);

        // Apply force to Rigidbody or character controller
        rb.velocity = Vector3.zero;
        rb.AddForce(forceVector, ForceMode.VelocityChange);
        Invoke("ResetJumpAnim", 0.5f);
    }

    void ResetJumpAnim()
    {
        anim.SetBool("isJumping", false);
    }

    private void Movement()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        if (!canWalk)
        {
            hor = 0;
            ver = 0;
        }

        movementDirection = new Vector3(hor, 0, ver);
        // Correct according to the camera direction
        movementDirection = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.y = 0;
        movementDirection.Normalize();

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        // Walk
        rb.velocity = new Vector3(movementDirection.x * currentSpeed, rb.velocity.y, movementDirection.z * currentSpeed);
    }

    public void TakeDamage(float amountOfDamage)
    {
        currentHp -= amountOfDamage;
        lastTimeTookDmg = Time.time;
        if (currentHp < 0)
            Death();
    }

    public void Death()
    {
        isDead = true;
        StartCoroutine(DeathAction()); // we do this bcz the enemies Patrol State script can't call coroutines, so we call a function that calls this coroutine instead
    }
    
    IEnumerator DeathAction()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        deathSound.Play();
        
        transitionAnimator.SetTrigger("Start");
        anim.enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(1f);

        GameInfo.SceneToLoad = "GameOver";
        GameInfo.SceneToUnload = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScreen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JumpArea"))
        {
            jumpUIPosition = other.transform.GetChild(0).transform;
            jumpUI.transform.position = jumpUIPosition.position;
            
            canJump = true;

            for (int i = 0; i < other.transform.parent.GetChild(0).childCount; i++)
            {
                jumpWaypoints[i] = other.transform.parent.GetChild(0).GetChild(i).transform;
            }
        }

        if (other.gameObject.CompareTag("HideArea"))
        {
            canHide = true;
            camGoalPosition = other.gameObject.transform.GetChild(0).GetComponent<Transform>();
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("Death"))
        {
            Death();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("JumpArea"))
        {
            canJump = false;
            jumpWaypoints = new Transform[8];
        }
        if (other.gameObject.CompareTag("HideArea"))
        {
            canHide = false;
            camGoalPosition = null;
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        // Draw Interact Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
        
        // Draw Meow Radius
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attractEnemiesRadius);
    }
}
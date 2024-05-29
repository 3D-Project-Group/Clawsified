using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Transform groundCheckStart;
    [SerializeField] private Transform groundCheckEnd;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

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
    [SerializeField] private float currentSpeed = 3f;
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

    [Header("Interact")]
    [SerializeField] private float interactRadius;
    [SerializeField] private LayerMask interactLayer;

    [Header("Public Bools")]
    public bool isHidden = false;
    public bool canHide = false;
    public bool canJump = false;
    public bool canWalk = false;
    public bool isResting = false;
    public bool isRunning = false;
    public bool doingPuzzle = false;

    [Header("Player UI")]
    [SerializeField] private Slider staminaWheel;
    [SerializeField] private Animator staminaWheelAnim;
    [Space]
    [SerializeField] private GameObject jumpText;
    [SerializeField] private GameObject hideText;
    [SerializeField] private GameObject unhideText;
    [Space]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text cheeseText;
    [SerializeField] private Slider cheeseCooldownSlider;
    [SerializeField] private Slider playerHpSlider; //Just for boss fight

    [Header("Cheats")]
    [SerializeField] private GameObject cheatList;
    public bool isInvisible;

    Transform[] jumpWaypoints = new Transform[8];
    Vector3 defaultGravity = new Vector3(0, -9.81f, 0);
    bool isJumping = false;

    Vector3 movementDirection;
    float hor;
    float ver;

    float lastTimeTookDmg;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        staminaWheel.maxValue = maxStamina;
        cheeseCooldownSlider.maxValue = cheeseCooldown; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
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
                pauseMenu.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.F) && canHide && !isHidden)
                StartCoroutine(Hide());
            else if (Input.GetKeyDown(KeyCode.F) && isHidden)
                UnHide();
            else if (Input.GetKeyDown(KeyCode.F))
                CallInteraction();

            if (!isHidden)
            {
                if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isResting)
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

                if (Input.GetKeyDown(KeyCode.Space) && canJump)
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

                canWalk = Physics.CheckCapsule(groundCheckStart.position, groundCheckEnd.position, groundCheckRadius, groundLayer) && !isJumping;
                if(canWalk && Physics.gravity != defaultGravity)
                    Physics.gravity = defaultGravity;
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

            if (isResting && currentStamina >= 50)
                isResting = false;

            if (currentStamina < maxStamina && !isRunning)
                currentStamina += 0.1f;

            if (isJumping && Physics.CheckCapsule(groundCheckStart.position, groundCheckEnd.position, groundCheckRadius, groundLayer))
                isJumping = false;

            if (canWalk)
                Movement();

            if (Time.time - lastTimeTookDmg >= playerRestoreHpTime && currentHp < maxHp)
                currentHp += Time.fixedDeltaTime;

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

        if(canJump)
            jumpText.SetActive(true);
        else
            jumpText.SetActive(false);

        if(isHidden)
        {
            hideText.SetActive(false);
            unhideText.SetActive(true);
        }
        else if (canHide)
        {
            unhideText.SetActive(false);
            hideText.SetActive(true);
        }
        else
        {
            unhideText.SetActive(false);
            hideText.SetActive(false);
        }
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
        
        GetComponent<MeshRenderer>().enabled = false;
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
        GetComponent<MeshRenderer>().enabled = true;
    }

    private void Jump(Vector3 goal)
    {
        isJumping = true;

        Vector3 direction = goal - transform.position;
        Vector3 newDirection = new Vector3(direction.x / 2, direction.y * 2, direction.z / 2 );
        newDirection.Normalize();

        // Scale direction vector by force magnitude
        Physics.gravity = new Vector3(0, -38, 0);
        Vector3 forceVector = newDirection * Mathf.Sqrt(2 * rb.mass * Physics.gravity.magnitude * (goal - transform.position).magnitude);

        // Apply force to Rigidbody or character controller
        rb.velocity = Vector3.zero;
        rb.AddForce(forceVector, ForceMode.VelocityChange);
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JumpArea"))
        {
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
        }
    }

    void OnDrawGizmos()
    {
        // Draw Interact Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
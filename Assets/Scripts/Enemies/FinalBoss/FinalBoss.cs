using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private int lastState = -1;
    public int currentState = -1; //0 = Waiting, 1 = Changing Tube, 2 = Attacking
    [Header("Movement")]
    public Transform[] tubeSpawnPoint;
    public Transform[] tubeEndingPoint;
    public int currentTube;
    [SerializeField] private float waitTimeToChangeTube;

    [Header("Stats")]
    public float bossMaxHp;
    public float bossCurrentHp;
    [SerializeField] private float speed;

    [SerializeField] private bool isWaiting = false;
    [SerializeField] private bool isSwitching = false;
    [SerializeField] private bool isAttacking = false;


    [SerializeField] private bool goingIn = false;
    [SerializeField] private bool goingOut = false;
    
    void Start()
    {
        currentTube = Random.Range(0, tubeSpawnPoint.Length);
        transform.position = tubeSpawnPoint[currentTube].position;
        bossCurrentHp = bossMaxHp;
    }

    void Update()
    {
        if (currentState == -1)
        {
            int i = Random.Range(0, 3);
            if (i != lastState)
            {
                currentState = i;
                lastState = currentState;
            }
        }

        if (currentState == 0 && !isWaiting)
        {
            StartCoroutine(Wait());
        }

        if(currentState == 1 && !isSwitching)
        {
            GoInside();
        }

        if( currentState == 2 && !isAttacking)
        {
            Attack();
        }

        if(isSwitching && goingIn)
        {
            Vector3 direction = tubeSpawnPoint[currentTube].position - transform.position;
            transform.Translate(direction * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, tubeSpawnPoint[currentTube].position) < 1)
            {
                goingIn = false;
                SwitchTube();
            }
        }
        else if(isSwitching && goingOut)
        {
            Vector3 direction = tubeEndingPoint[currentTube].position - transform.position;
            transform.Translate(direction * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, tubeEndingPoint[currentTube].position) < 1)
            {
                isSwitching = false;
                currentState = -1;
            }
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
        currentTube = Random.Range(0, tubeSpawnPoint.Length);
        transform.position = tubeSpawnPoint[currentTube].position;
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
        print("Attack");
        currentState = -1;
        isAttacking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public int currentState = -1; //0 = Waiting, 1 = Changing Tube, 2 = Attacking
    [Header("Movement")]
    public Transform[] tubeSpawnPoint;
    public Transform[] tubeEndingPoint;
    public int currentTube;
    [SerializeField] private float waitTimeToChangeTube;

    [Header("Stats")]
    public float bossMaxHp;
    public float bossCurrentHp;

    private bool isWaiting = false;
    private bool isSwitching = false;
    private bool isAttacking = false;
    
    void Start()
    {
        bossCurrentHp = bossMaxHp;
    }

    void Update()
    {
        if (currentState == -1)
            currentState = Random.Range(1, 3);

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

        }
    }

    void GoInside()
    {
        currentState = 1;
        isSwitching = true;

        Vector3 direction = tubeSpawnPoint[currentTube].position - transform.position;
        transform.Translate(direction);

        if (Vector3.Distance(transform.position, tubeSpawnPoint[currentTube].position) < 1)
        {
            SwitchTube();
        }
    }

    void GoOutside()
    {
        Vector3 direction = tubeEndingPoint[currentTube].position - transform.position;
        transform.Translate(direction); 

        if (Vector3.Distance(transform.position, tubeEndingPoint[currentTube].position) < 1)
        {
            isSwitching = false;
            currentState = -1;
        }
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
        currentState = -1;
        isAttacking = false;
    }
}

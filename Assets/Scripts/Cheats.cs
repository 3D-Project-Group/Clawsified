using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void Teleport(Transform goal)
    {
        player.transform.position = goal.position;
    }

    public void Invisible() //Does not work on boss
    {
        player.GetComponent<PlayerController>().isInvisible = !player.GetComponent<PlayerController>().isInvisible;
    }
    
    public void GoToBossRoom()
    {
        SceneManager.LoadSceneAsync("BossScene");
    }

    public void BossKill()
    {
        GameObject.Find("Boss").GetComponent<FinalBoss>().TakeDamage(GameObject.Find("Boss").GetComponent<FinalBoss>().bossCurrentHp);
    }
}

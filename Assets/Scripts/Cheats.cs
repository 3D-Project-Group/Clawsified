using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
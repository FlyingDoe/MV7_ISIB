using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisardSpawner : MonoBehaviour
{
    public BoxCollider territory;
    GameObject player;
    bool playerInTerritory;

    public GameObject enemyx;
    EnemyLisard enemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = enemyx.GetComponent<EnemyLisard>();
        playerInTerritory = false;
    }

    void Update()
    {
        enemy.test();

        if (playerInTerritory == true)
        {
            enemy.MoveToPlayer();
        }

        if (playerInTerritory == false)
        {
            //rien faire
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = false;
        }
    }
}

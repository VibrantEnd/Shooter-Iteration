using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float statMultf = 1;
    public int statMulti = 1;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    EnemyAttack enemyAttack;
    EnemyHealth enemyHealth;
    EnemyMovement enemyMovement;
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        InvokeRepeating("StatUps", 5f, 5f);
    }
    void StatUps()
    {
        statMultf += .2f;
        Debug.Log(statMultf);
        statMulti += 1 / 5;
    }
    void Spawn()
    {
        if (playerHealth.currentHealth <= 0)
        {
            return;
        }
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        
        if (spawnTime > .5)
        {
            spawnTime = spawnTime - (Mathf.Sqrt(statMultf));
        }
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        enemyMovement = GetComponentInChildren<EnemyMovement>();
        enemyMovement.player = player.transform;
        enemyAttack.player = player;
    }

}

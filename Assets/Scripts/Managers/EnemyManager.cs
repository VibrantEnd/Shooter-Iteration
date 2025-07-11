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
    EnemyHealth enemyHealth;

    private void Start()
    {
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
    }

}

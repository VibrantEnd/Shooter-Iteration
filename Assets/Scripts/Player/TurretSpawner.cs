using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject turrent;
    public Vector3 spawnPoint = new Vector3(0, 2f, 0);
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)/*0&& ScoreManager.score >= 500*/)
        {
            ScoreManager.score -= 500;
            Instantiate(turrent, (transform.position + spawnPoint), transform.rotation);
        }
    }
}

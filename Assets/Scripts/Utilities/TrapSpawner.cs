using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject trap;
    private void Awake()
    {
        InvokeRepeating("Spawn", 30f, 30f);
    }
    private IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(30f);
        Instantiate(trap, transform.position, transform.rotation);
    }
    private void Spawn()
    {
        Instantiate(trap, transform.position, transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

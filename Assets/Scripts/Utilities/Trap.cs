using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    GameObject player;
    bool playerInRange;
    PlayerMovement playerMovement;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerMovement.trapped = true;
            StartCoroutine(CountdownTimer());
        }
    }
    private IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(5f);
        playerMovement.trapped = false;
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

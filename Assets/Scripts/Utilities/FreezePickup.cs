using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePickup : MonoBehaviour
{
    GameObject player;
    PlayerHealth playerHealth;
    bool inrange;
    BoxCollider capsuleCollider;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        capsuleCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Frozen!");
            playerHealth.freezeRange = true;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

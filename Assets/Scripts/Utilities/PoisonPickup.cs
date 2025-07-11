using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPickup : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    PlayerHealth playerHealth;
    bool inrange;
    SphereCollider capsuleCollider;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        capsuleCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Poisoned!");
            playerHealth.poisonRange = true;
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

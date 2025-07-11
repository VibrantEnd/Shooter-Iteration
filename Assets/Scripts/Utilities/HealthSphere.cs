using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSphere : MonoBehaviour
{
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
            playerHealth.healRange = true;
            Destroy(gameObject);
        }
    }
    public void Heal()
    {
        Destroy(gameObject);
    }
    
    void Update()
    {

            
        
    }
}

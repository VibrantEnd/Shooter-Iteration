using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = .5f;
    public int attackDamage = 10;

    private Animator anim;
    public GameObject player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private PlayerMovement playerMovement;
    private EnemyManager enemyManager;

    private EnemyHealth electrified;
    private EnemyHealth frozen;
    private bool playerInRange;
    private float timer;

    private void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0 && !enemyHealth.electrified && !enemyHealth.frozen) 
        {
            Attack();
        }
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("PlayerDead");
        }
    }

    void Attack()
    {
        timer = 0f;
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}

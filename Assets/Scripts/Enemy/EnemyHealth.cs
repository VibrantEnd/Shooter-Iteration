using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue;
    public AudioClip deathClip;

    public bool frozen;
    public bool poisoned;
    public bool electrified;
    private int level;
    private int elecTimer;
    private int frozTimer;
    int statMulti;
    float statMultf;

    private Material[] material;
    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    Material hitParticleColor;
    Transform enemyTransform;
    public GameObject healOrb;
    public GameObject drop;
    public Vector3 spawnPoint = new Vector3(0, .5f, 0);

    bool isDead;
    bool isSinking;

    private void Awake()
    {
        material = GetComponentInChildren<Renderer>().materials;
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        hitParticleColor = GetComponentInChildren<ParticleSystemRenderer>().material;
        capsuleCollider = GetComponent<CapsuleCollider>();
        enemyTransform = transform;
        currentHealth = startingHealth;

        InvokeRepeating("StatUps",5f,5f);
    }

    void Update()
    {
        
        
        if (currentHealth <= 0 && !isDead)
        {
            GetComponent<Animator>().enabled = true;
            Death();
        }
        if (isSinking)
        
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        if (frozTimer <= 0 && !isDead && elecTimer <= 0 && !poisoned)
        {
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<Animator>().enabled = true;
            frozen = false;
            electrified = false;
            CancelInvoke("FrozTimer");
            CancelInvoke("ElecTimer");
            material[1].color = Color.white;
            material[0].color = Color.white;
        }
        if ((frozTimer >= 0 && isDead)||(elecTimer >= 0 && isDead))
        {
            GetComponent<Animator>().enabled = true;
        }
    }
    void StatUps()
    {
        statMultf += .2f;
        Debug.Log(statMultf);
        statMulti += 1;
        Debug.Log(statMulti + "i");
        scoreValue += scoreValue + (scoreValue * (statMulti / 10));
        startingHealth = startingHealth + (startingHealth * (statMulti/10));
    }
    void TakePoison()
    { 
        currentHealth -= (level);
        Debug.Log(currentHealth);
     
    }
    void ElecTimer()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            elecTimer -= 1;
            currentHealth -= 5;
            Debug.Log(elecTimer);
        }
        
    }
    void FrozTimer()
    {
        if (isDead)
        {
            return;
        }
        else
        {
            frozTimer -= 1;
        }
    }
    public void Poisoned()
    {
        if (poisoned == false)
        {
            material[1].color = Color.green;
            material[0].color = Color.green;
            poisoned = true;
            level += 5;
            InvokeRepeating("TakePoison", 1f, 1f);
        }
        else if (poisoned == true)
        {
            level += 5;
            Debug.Log(level);
        }

    }
    public void Electrified()
    {
        if (electrified == false)
        {
            electrified = true;
            elecTimer = 1;
            material[1].color = Color.yellow;
            material[0].color = Color.yellow;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().enabled = false;
            InvokeRepeating("ElecTimer", 1f, 1f);
        }
        if (electrified)
        {
            return;
        }
    }
    public void Frozen()
    {
        if (frozen == false)
        {
            frozen = true;
            frozTimer = 5;
            material[1].color = Color.blue;
            material[0].color = Color.blue;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Animator>().enabled = false;
            InvokeRepeating("FrozTimer", 1f, 1f);
        }
        if (frozen)
        {
            return;
        }
    }
    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if (isDead) return;

        enemyAudio.Play();

        currentHealth -= amount;
        if (poisoned)
        {
            hitParticleColor.color = Color.green;
        }
        if (electrified)
        {
            hitParticleColor.color = Color.yellow;
        }
        if (frozen)
        {
            hitParticleColor.color = Color.blue;
        }
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            Death();
        }
        
        
    }

    private void Death()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        GetComponent<Animator>().enabled = true;
        anim.SetTrigger("Dead");
        enemyAudio.clip = deathClip;
        enemyAudio.Play();
        int randomInt = UnityEngine.Random.Range(1, 101);
        if (randomInt >= 96)
        {
            Instantiate(drop, (transform.position + spawnPoint), transform.rotation);
        }
        else if (60 >= randomInt && randomInt<= 95)
        {
            Instantiate(healOrb, (transform.position + spawnPoint), transform.rotation);
        }
    }

    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue *(1+(statMulti/10));
        ScoreManager.Instance.ShowScore();
        Destroy(gameObject, 2f);
    }
}

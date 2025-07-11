using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public Image sprintUI;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f,  .1f);
    public Color flashHealColour = new Color(0f, 1f, 0f, .1f);
    public Color flashColorElectricity = new Color(1f, 1f, 0);
    public Color flashColorPoison = new Color(.2f, 1f, 0);
    public Color flashColorFreeze = new Color(0f, 0f, 1);
    public GameObject healOrb;
    public GameObject thunderPickup;
    public GameObject poisonPickup;
    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    public bool healRange;
    public bool elecRange;
    public bool poisonRange;
    public bool freezeRange;
    bool healed;

    private void Awake()
    {
        healOrb = GameObject.FindGameObjectWithTag("HealthSphere");
        thunderPickup = GameObject.FindGameObjectWithTag("ThunderPickup");
        poisonPickup = GameObject.FindGameObjectWithTag("PoisonPickup");
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
    }
    void Update()
    {
        if (playerMovement.canSprint)
        {
            sprintUI.color = Color.white;

        }
        else
        {
            sprintUI.color = new Color(0, 0, 0, .5f);
        }
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        if (healRange)
        {
            damageImage.color = flashHealColour;
            Debug.Log("healed");
            currentHealth += 20;
            if (currentHealth >= 100)
            {
                currentHealth = 100;
            }
            healthSlider.value = currentHealth;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, (flashSpeed/5) * Time.deltaTime);
        }
        healRange = false;
        if (elecRange)
        {
            Debug.Log("Electrified!");
            damageImage.color = flashColorElectricity;
            playerShooting.electricity = true;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, (flashSpeed / 5) * Time.deltaTime);
        }
        elecRange = false;
        if (poisonRange)
        {
            Debug.Log("Poisoned!");
            damageImage.color = flashColorPoison;
            playerShooting.poison = true;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, (flashSpeed / 5) * Time.deltaTime);
        }
        poisonRange = false;
        if (freezeRange)
        {
            Debug.Log("Frozen!");
            damageImage.color = flashColorFreeze;
            playerShooting.ice = true;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, (flashSpeed / 5) * Time.deltaTime);
        }
        freezeRange = false;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == healOrb)
        {
            healRange = true;
        }
        if (other.gameObject == thunderPickup)
        {
            elecRange = true;
        }
        if (other.gameObject == poisonPickup)
        {
            poisonRange = true;
        }
    }
    public void TakeDamage (int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        playerAudio.Play();
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        anim.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        Debug.Log("Restart Level");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float timeBetweenShotgun = .5f;
    public float shotgunTimer;
    public float range = 100f;
    public int shotgunAmount = 6;
    public float shotgunSpread = 10f;

    public bool poison = false;
    public bool ice = false;
    public bool electricity = false;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    public LineRenderer shotgunLineRenderer;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = .2f;

    private void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }
    public void ShootTarget(Transform target, int damageOverride = -1)
    {
        int damage = (damageOverride >= 0) ? damageOverride : damagePerShot;
        Vector3 direction = (target.position - transform.position).normalized;
        shootRay.origin = transform.position;
        shootRay.direction = direction;

        gunAudio.Play();

        gunLight.enabled = true;
        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, shootHit.point);
            }
            gunLine.SetPosition(1, shootHit.point);



        }
        else gunLine.SetPosition(1, transform.position + direction * range);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            electricity = false;
            ice = false;
            poison = true;
            Debug.Log("Poison = true");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            electricity = false;
            ice = true;
            poison = false;
            Debug.Log("Ice = true");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            electricity = true;
            ice = false;
            poison = false;
            Debug.Log("Electricity = true");
        }
        timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }
        shotgunTimer += Time.deltaTime;
        if (Input.GetButton("Fire2")&& shotgunTimer >= timeBetweenShotgun)
        {
            shotgunTimer = 0f;
            ShootShotgun();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime&& shotgunTimer >= timeBetweenShotgun *effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
    void ShootShotgun()
    {
        gunAudio.Play();
        gunLight.enabled = true;
        gunParticles.Play();
        
        for (int i = 0; i < shotgunAmount; i++)
        {
            Quaternion spreadAngle = Quaternion.Euler(UnityEngine.Random.Range(-shotgunSpread, shotgunSpread), UnityEngine.Random.Range(-shotgunSpread, shotgunSpread), 0);
            Vector3 direction = spreadAngle * transform.forward;
            LineRenderer lineRenderer = Instantiate(shotgunLineRenderer, transform.position, Quaternion.identity);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * range);
            Destroy(lineRenderer.gameObject, effectsDisplayTime);
            
            
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range, shootableMask, QueryTriggerInteraction.Collide))
            {
                EnemyHealth eh = hit.collider.GetComponent<EnemyHealth>();
                if (eh != null) eh.TakeDamage(damagePerShot, hit.point);
                gunLine.SetPosition(1, hit.point);

            }
            


        }
        
    
    }
    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;
        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;


        if(Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                if (poison)
                {
                    enemyHealth.Poisoned();
                }
                if (electricity)
                {
                    enemyHealth.Electrified();
                }
                if (ice)
                {
                    enemyHealth.Frozen();
                }
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }

    }
}

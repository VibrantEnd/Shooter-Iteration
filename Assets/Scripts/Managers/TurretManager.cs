using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    PlayerShooting playerShooting;
    int shootableMask;
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = .15f;
    bool targeted;
    Rigidbody rigidBody;
    public Vector3 spawnPoint = new Vector3(0, -1.5f, 0);
    float timer = 0f;
    SphereCollider sphereCollider;
    Ray shootRay;
    GameObject enemy;
    private List<Transform> enemies = new List<Transform>();

    private void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        playerShooting = GetComponentInChildren<PlayerShooting>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = range;
        rigidBody.isKinematic = true;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        StartCoroutine(nameof(CountdownTimer));
    }
    private void Update()
    {
        timer += Time.deltaTime;

        enemies.RemoveAll(enemy => enemy == null);
        Transform nearest = null;
        float minSqr = range * range;
        foreach (var enemy in enemies)
        {
            float distance = (enemy.position - (transform.position-spawnPoint)).sqrMagnitude;
            if (distance < minSqr)
            {
                minSqr = distance;
                nearest = enemy;
            }
        }
        if (nearest != null && timer >= timeBetweenBullets)
        {
            playerShooting.ShootTarget(nearest);
            Debug.Log("ShootedEnemy");
            timer = 0f;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered by" + other.name + "tag = " + other.tag);
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.transform);
        }
    }

    private IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}

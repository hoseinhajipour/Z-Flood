using UnityEngine;
using System.Collections;

public class TurretBotController : MonoBehaviour
{
    [Header("Turret Settings")]
    public float rotationSpeed = 5f;
    public float scanRadius = 15f;
    public float scanInterval = 0.5f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;
    public int damage = 10;
    public float accuracy = 0.95f; // 95% accuracy

    [Header("Visual Effects")]
    public GameObject destructionEffect;
    public GameObject scanEffect;
    public GameObject muzzleFlash;
    public GameObject hitEffect;

    private Transform currentTarget;
    private float nextFireTime;
    private float nextScanTime;

    void Start()
    {
        // Start scanning for enemies
        StartCoroutine(ScanForEnemies());
    }

    IEnumerator ScanForEnemies()
    {
        while (true)
        {
            // Find nearest enemy
            Collider[] enemies = Physics.OverlapSphere(transform.position, scanRadius);
            float closestDistance = float.MaxValue;
            Transform closestEnemy = null;

            foreach (var enemy in enemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy.transform;
                    }
                }
            }

            currentTarget = closestEnemy;

            // Show scan effect
            if (scanEffect != null)
            {
                GameObject effect = Instantiate(scanEffect, transform.position, Quaternion.identity);
                effect.transform.localScale = new Vector3(scanRadius * 2, 1, scanRadius * 2);
                Destroy(effect, 0.5f);
            }

            yield return new WaitForSeconds(scanInterval);
        }
    }

    void Update()
    {
        if (currentTarget != null)
        {
            // Rotate towards target
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            direction.y = 0; // Keep turret level
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if we can fire
            if (Time.time >= nextFireTime)
            {
                FireAtTarget();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void FireAtTarget()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Calculate direction to target with some randomness for accuracy
        Vector3 direction = (currentTarget.position - firePoint.position).normalized;
        direction += new Vector3(
            Random.Range(-1f, 1f) * (1f - accuracy),
            Random.Range(-1f, 1f) * (1f - accuracy),
            Random.Range(-1f, 1f) * (1f - accuracy)
        );
        direction.Normalize();

        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Set bullet damage
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.damage = damage;
        }

        // Show muzzle flash
        if (muzzleFlash != null)
        {
            GameObject flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            flash.transform.parent = firePoint;
            Destroy(flash, 0.1f);
        }
    }

    public void PlayDestructionEffect()
    {
        if (destructionEffect != null)
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw scan radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scanRadius);

        // Draw fire point
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }
} 
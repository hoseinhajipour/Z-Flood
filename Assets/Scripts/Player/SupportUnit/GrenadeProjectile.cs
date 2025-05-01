using UnityEngine;
using System.Collections;

public class GrenadeProjectile : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float explosionRadius = 3f;
    public int damage = 10;
    public float fuseTime = 3f;
    public GameObject explosionPrefab;
    public GameObject explosionRadiusIndicator;

    private bool hasExploded = false;

    void Start()
    {
        // Start the fuse timer
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Create explosion effect
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        // Show explosion radius indicator
        if (explosionRadiusIndicator != null)
        {
            GameObject indicator = Instantiate(explosionRadiusIndicator, transform.position, Quaternion.identity);
            Destroy(indicator, 0.5f);
        }

        // Deal damage to enemies in radius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    // Calculate damage falloff based on distance
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    float damageMultiplier = 1f - (distance / explosionRadius);
                    int finalDamage = Mathf.RoundToInt(damage * damageMultiplier);
                    
                    enemyAI.TakeDamage(finalDamage);
                }
            }
        }

        // Clean up
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Explode on impact with certain objects
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
} 
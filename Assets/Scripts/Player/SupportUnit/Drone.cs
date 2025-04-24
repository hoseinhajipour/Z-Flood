using UnityEngine;

public class Drone : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the drone
    public Vector3 offset = new Vector3(0, 3, 0); // Offset for drone's position relative to the player
    public float detectionRange = 10f; // Detection range for zombies
    public float fireRate = 1f; // Fire rate
    public int damage = 1; // Bullet damage
    public float bulletSpeed = 10f; // Bullet speed

    public Transform player; // Reference to the player
    public GameObject bulletPrefab; // Bullet prefab
    private float nextFireTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        FollowPlayer();
        CheckAndShootZombies();
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            // Move drone towards the player with an offset
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void CheckAndShootZombies()
    {
        // Check for zombies within the detection range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Fire at the zombie
                if (Time.time >= nextFireTime)
                {
                    FireAtZombie(hitCollider.transform);
                    nextFireTime = Time.time + 1f / fireRate;
                }
                break; // Only fire at the first zombie detected
            }
        }
    }

    void FireAtZombie(Transform zombie)
    {
        if (bulletPrefab != null)
        {
            // Direction to the zombie
            Vector3 direction = (zombie.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Set bullet velocity
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
            }

            // Set bullet rotation towards the zombie
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Draw detection range in the scene view using Gizmos
    void OnDrawGizmos()
    {
        // Set the Gizmos color to yellow
        Gizmos.color = Color.yellow;

        // Draw a sphere to represent the detection range
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

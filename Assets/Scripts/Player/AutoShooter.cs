using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float detectionRange = 10f;
    public float BulletSpeed = 10f;
    private float nextFireTime = 0f;

    void Update()
    {
        GameObject target = FindClosestEnemy();
        if (target != null && Time.time >= nextFireTime)
        {
            Shoot(target.transform.position);
            nextFireTime = Time.time + fireRate;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = detectionRange;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position); // Use Vector3.Distance for 3D
            if (dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }
        return closest;
    }

    void Shoot(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - firePoint.position).normalized; // Direction vector to target
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = dir * BulletSpeed; // Use Rigidbody for 3D physics

        // Calculate the angle in 3D space
        Quaternion targetRotation = Quaternion.LookRotation(dir); // LookRotation for 3D
        bullet.transform.rotation = targetRotation;
    }
}

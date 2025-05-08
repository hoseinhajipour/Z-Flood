using UnityEngine;

public class MachineGunTower : BaseTower
{
    [Header("Machine Gun Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float spreadAngle = 5f;

    protected override void Attack()
    {
        if (currentTarget == null) return;

        // Calculate direction to target
        Vector3 direction = (currentTarget.transform.position - firePoint.position).normalized;
        
        // Add random spread
        direction = Quaternion.Euler(
            Random.Range(-spreadAngle, spreadAngle),
            Random.Range(-spreadAngle, spreadAngle),
            0
        ) * direction;

        // Create and setup bullet
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
            bulletComponent.damage = (int)damage;
        }
    }
} 
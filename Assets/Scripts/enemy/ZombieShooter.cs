using UnityEngine;

public class ZombieShooter : EnemyAI
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootRange = 6f;
    public float fireRate = 1.5f;

    private float nextShootTime = 0f;

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= shootRange && Time.time >= nextShootTime)
        {
            ShootAtPlayer();
            nextShootTime = Time.time + fireRate;
        }
    }

    void ShootAtPlayer()
    {
        Vector3 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * 6f;
    }
}

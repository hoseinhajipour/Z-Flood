using UnityEngine;

public class TurretBot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int damage = 15;
    public float fireRate = 1f;
    public float range = 8f;
    public Transform target;

    private float nextFireTime;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy")?.transform;
    }

    void Update()
    {
        if (target != null && Time.time >= nextFireTime)
        {
            FireAtEnemy();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireAtEnemy()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;
            bullet.GetComponent<Bullet>().damage = damage;
        }
    }

    public void UpgradeTurret(float fireRateIncrease, int damageIncrease)
    {
        fireRate += fireRateIncrease;
        damage += damageIncrease;
    }
}

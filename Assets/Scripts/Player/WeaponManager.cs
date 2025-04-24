using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponData[] availableWeapons;
    private int currentWeaponIndex = 0;
    private float nextFireTime;

    private Transform target;
    private Transform firePoint;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy")?.transform;
        firePoint = transform.Find("FirePoint"); // حتماً یه child به نام FirePoint داشته باش
    }

    void Update()
    {
        if (target == null)
        {
            FindClosestEnemy();
            return;
        }

        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / availableWeapons[currentWeaponIndex].fireRate;
        }
    }

    void Fire()
    {
        var weapon = availableWeapons[currentWeaponIndex];
        Vector2 dir = (target.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(weapon.bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = dir * weapon.bulletSpeed;
        bullet.GetComponent<Bullet>().damage = weapon.damage;
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = float.MaxValue;
        GameObject closest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        if (closest != null)
            target = closest.transform;
    }

    public void UpgradeWeapon(int index)
    {
        if (index >= 0 && index < availableWeapons.Length)
            currentWeaponIndex = index;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTower : MonoBehaviour
{
    public float attackRange = 10f;
    public GameObject bulletPrefab;
    public int shotsPerMinute = 60;

    [SerializeField]
    private Transform muzzlePrefab;
    private Transform muzzleInstance;
    private float timeSinceLastShot;

    private void Start()
    {
        muzzleInstance = Instantiate(muzzlePrefab, transform.position, Quaternion.identity).transform;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        float fireInterval = 60f / shotsPerMinute;

        if (timeSinceLastShot >= fireInterval)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

            foreach (var enemy in hitColliders)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    ShootAt(enemy.transform);
                    timeSinceLastShot = 0f;
                    break;
                }
            }
        }
    }

    private void ShootAt(Transform target)
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzleInstance.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = (target.position - muzzleInstance.position).normalized * 10f;
    }
}
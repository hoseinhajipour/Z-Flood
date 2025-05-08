using UnityEngine;
using System.Collections;

public class BaseTower : MonoBehaviour
{
    [Header("Tower Settings")]
    public int cost = 100;
    public int level = 1;
    public float range = 10f;
    public float fireRate = 1f;
    public float damage = 10f;
    public float upgradeCostMultiplier = 1.5f;

    [Header("Targeting")]
    public LayerMask enemyLayer;
    public Transform firePoint;
    public GameObject currentTarget;

    private float nextFireTime;

    protected virtual void Start()
    {
        nextFireTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FindTarget();
            if (currentTarget != null)
            {
                Attack();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    protected virtual void FindTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.gameObject;
            }
        }

        currentTarget = closestEnemy;
    }

    protected virtual void Attack()
    {
        // Base attack method to be overridden by specific tower types
    }

    public virtual void Upgrade()
    {
        level++;
        range *= 1.2f;
        fireRate *= 0.9f;
        damage *= 1.3f;
    }

    public int GetUpgradeCost()
    {
        return Mathf.RoundToInt(cost * upgradeCostMultiplier * level);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
} 
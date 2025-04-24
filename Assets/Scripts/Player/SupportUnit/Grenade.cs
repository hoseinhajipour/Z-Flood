using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float throwInterval = 5f;
    public int damage = 10;
    public float explosionRadius = 3f;
    public float throwForce = 10f;
    private float nextThrowTime;

    void Update()
    {
        if (Time.time >= nextThrowTime)
        {
            ThrowGrenade();
            nextThrowTime = Time.time + throwInterval;
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(Resources.Load("GrenadePrefab"), transform.position, Quaternion.identity) as GameObject;
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * throwForce, ForceMode2D.Impulse);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(grenade.transform.position, explosionRadius);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(damage);
            }
        }
    }

    public void UpgradeGrenade(float newThrowInterval, int damageIncrease)
    {
        throwInterval = newThrowInterval;
        damage += damageIncrease;
    }
}

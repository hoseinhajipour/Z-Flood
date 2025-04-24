using UnityEngine;

public class AirStrike : MonoBehaviour
{
    public float strikeInterval = 10f;
    public int damage = 30;
    public float strikeRadius = 5f;

    private float nextStrikeTime;

    void Update()
    {
        if (Time.time >= nextStrikeTime)
        {
            PerformAirStrike();
            nextStrikeTime = Time.time + strikeInterval;
        }
    }

    void PerformAirStrike()
    {
        Vector2 randomPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Collider2D[] enemies = Physics2D.OverlapCircleAll(randomPos, strikeRadius);

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(damage);
            }
        }
    }

    public void UpgradeAirStrike(float newStrikeInterval, int damageIncrease)
    {
        strikeInterval = newStrikeInterval;
        damage += damageIncrease;
    }
}

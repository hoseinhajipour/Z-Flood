using UnityEngine;

public class BossEnemy : EnemyAI
{
    public GameObject specialAttackPrefab;
    public float specialAttackInterval = 5f;

    private float nextAttackTime = 0f;

    protected override void Update()
    {
        base.Update();

        if (Time.time >= nextAttackTime)
        {
            SpecialAttack();
            nextAttackTime = Time.time + specialAttackInterval;
        }
    }

    void SpecialAttack()
    {
        Instantiate(specialAttackPrefab, transform.position, Quaternion.identity);
        // می‌تونی اینجا موجی از گلوله یا انفجار ایجاد کنی
    }
}

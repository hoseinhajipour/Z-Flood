using UnityEngine;

public class BirdSimple : EnemyAI
{
    protected override void MoveTowardPlayer()
    {
        if (player == null) return;
        // پرنده سریع‌تر حرکت کنه
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)dir * (moveSpeed * 1.5f) * Time.deltaTime;
    }
}

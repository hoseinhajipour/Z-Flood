using UnityEngine;

public class Drone : MonoBehaviour
{
    public float moveSpeed = 5f; // سرعت حرکت پهباد
    public Vector3 offset = new Vector3(0, 3, 0); // فاصله از بازیکن
    public float detectionRange = 10f; // محدوده تشخیص دشمن
    public float fireRate = 1f; // سرعت تیراندازی
    public int damage = 1; // میزان آسیب گلوله
    public float bulletSpeed = 10f; // سرعت گلوله
    public float rotationSpeed = 500f; // سرعت چرخش پهباد
    public float hoverSpeed = 2f; // سرعت نوسان عمودی
    public float hoverHeight = 0.5f; // ارتفاع نوسان

    public Transform player; // مرجع بازیکن
    public GameObject bulletPrefab; // پرتابه گلوله
    private float nextFireTime;
    private Transform currentTarget;
    private float hoverOffset;
    private bool isShooting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        hoverOffset = Random.Range(0f, 2f * Mathf.PI); // مقدار تصادفی برای نوسان
    }

    void Update()
    {
        FollowPlayer();
        CheckAndShootZombies();
        ApplyHoverEffect();
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            // حرکت به سمت بازیکن با فاصله مشخص
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void ApplyHoverEffect()
    {
        // نوسان عمودی
        float hoverY = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y + hoverY, transform.position.z);
    }

    void CheckAndShootZombies()
    {
        // جستجوی دشمن در محدوده مشخص
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform closestZombie = null;
        float closestDistance = detectionRange;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestZombie = hitCollider.transform;
                    closestDistance = distance;
                }
            }
        }

        if (closestZombie != null)
        {
            currentTarget = closestZombie;
            isShooting = true;
            RotateTowardsTarget();

            if (Time.time >= nextFireTime)
            {
                FireAtZombie(closestZombie);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            isShooting = false;
            // چرخش به سمت بازیکن در صورت عدم وجود دشمن
            if (player != null)
            {
                Vector3 lookDirection = (player.position - transform.position).normalized;
                lookDirection.y = 0;
                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireAtZombie(Transform zombie)
    {
        if (bulletPrefab != null)
        {
            Vector3 direction = (zombie.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.velocity = direction * bulletSpeed;
            }

            // تنظیم چرخش گلوله به سمت دشمن
            bullet.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // نمایش محدوده تشخیص در صحنه
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float detectionRange = 10f;
    public float BulletSpeed = 10f;
    public float rotationSpeed = 700f; // سرعت چرخش به سمت دشمن
    public Material rangeMaterial; // متریال برای نمایش محدوده تشخیص
    private float nextFireTime = 0f;
    private GameObject currentTarget;
    private bool isShooting = false;
    private DetectionRangeVisualizer rangeVisualizer;

    void Start()
    {
        // اضافه کردن ویژوالایزر محدوده تشخیص
        rangeVisualizer = gameObject.AddComponent<DetectionRangeVisualizer>();
        rangeVisualizer.detectionRange = detectionRange;
        rangeVisualizer.rangeMaterial = rangeMaterial; // تنظیم متریال
    }

    void Update()
    {
        currentTarget = FindClosestEnemy();
        if (currentTarget != null)
        {
            // چرخش به سمت دشمن
            RotateTowardsTarget();
            
            // تیراندازی
            if (Time.time >= nextFireTime)
            {
                Shoot(currentTarget.transform.position);
                nextFireTime = Time.time + fireRate;
                isShooting = true;
            }
        }
        else
        {
            isShooting = false;
        }

        // نمایش/مخفی کردن محدوده تشخیص
        rangeVisualizer.SetVisibility(Input.GetKey(KeyCode.V)); // با فشار دادن کلید V محدوده نمایش داده می‌شود
    }

    void RotateTowardsTarget()
    {
        if (currentTarget == null) return;

        // محاسبه جهت به سمت دشمن
        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        direction.y = 0; // فقط چرخش در صفحه XZ

        // محاسبه زاویه چرخش
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        // چرخش نرم به سمت دشمن
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = detectionRange;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                closest = enemy;
                minDist = dist;
            }
        }
        return closest;
    }

    void Shoot(Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = dir * BulletSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        bullet.transform.rotation = targetRotation;
    }

    // متد برای بررسی وضعیت تیراندازی
    public bool IsShooting()
    {
        return isShooting;
    }
}

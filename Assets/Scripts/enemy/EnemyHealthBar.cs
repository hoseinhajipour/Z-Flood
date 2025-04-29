using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healthBarPrefab; // پرتابه نوار سلامت
    public Vector3 offset = new Vector3(0, 2f, 0); // فاصله از سر دشمن
    public float smoothSpeed = 5f; // سرعت نرم شدن تغییرات نوار سلامت

    private GameObject healthBarInstance;
    private Image healthFillImage;
    private Health enemyHealth;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        enemyHealth = GetComponent<Health>();

        // ایجاد نوار سلامت
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + offset, Quaternion.identity);
            healthBarInstance.transform.SetParent(transform); // قرار دادن نوار سلامت به عنوان فرزند دشمن
            
            // پیدا کردن کامپوننت Image
            healthFillImage = healthBarInstance.GetComponentInChildren<Image>();
            if (healthFillImage != null)
            {
                // تنظیم نوع Fill
                healthFillImage.type = Image.Type.Filled;
                healthFillImage.fillMethod = Image.FillMethod.Horizontal;
                healthFillImage.fillOrigin = 0; // Left to Right
            }

            // تنظیم مقدار اولیه نوار سلامت
            if (healthFillImage != null && enemyHealth != null)
            {
                UpdateHealthBar();
            }
        }
    }

    void Update()
    {
        if (healthBarInstance != null && mainCamera != null)
        {
            // چرخش نوار سلامت به سمت دوربین
            healthBarInstance.transform.LookAt(healthBarInstance.transform.position + mainCamera.transform.rotation * Vector3.forward,
                                             mainCamera.transform.rotation * Vector3.up);

            // به‌روزرسانی موقعیت نوار سلامت
            healthBarInstance.transform.position = transform.position + offset;

            // به‌روزرسانی مقدار نوار سلامت به صورت نرم
            if (healthFillImage != null && enemyHealth != null)
            {
                float targetFill = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
                healthFillImage.fillAmount = Mathf.Lerp(healthFillImage.fillAmount, targetFill, smoothSpeed * Time.deltaTime);
            }
        }
    }

    void UpdateHealthBar()
    {
        if (healthFillImage != null && enemyHealth != null)
        {
            float fillAmount = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
            healthFillImage.fillAmount = fillAmount;
        }
    }

    void OnDestroy()
    {
        // حذف نوار سلامت هنگام نابودی دشمن
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }
    }
} 
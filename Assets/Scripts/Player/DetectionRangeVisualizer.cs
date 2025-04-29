using UnityEngine;

public class DetectionRangeVisualizer : MonoBehaviour
{
    public float detectionRange = 10f; // محدوده تشخیص
    public Material rangeMaterial; // متریال برای نمایش محدوده
    public float fadeSpeed = 2f; // سرعت محو شدن

    private GameObject rangeVisualizer;
    private float currentAlpha = 0f;
    private bool isVisible = false;

    void Start()
    {
        // ایجاد صفحه نمایش محدوده
        rangeVisualizer = GameObject.CreatePrimitive(PrimitiveType.Quad);
        rangeVisualizer.name = "DetectionRangeVisualizer";
        rangeVisualizer.transform.SetParent(transform);
        rangeVisualizer.transform.localPosition = new Vector3(0, 0.1f, 0); // کمی بالاتر از زمین
        rangeVisualizer.transform.localRotation = Quaternion.Euler(90, 0, 0); // چرخش به سمت بالا
        rangeVisualizer.transform.localScale = new Vector3(detectionRange * 2, detectionRange * 2, 1);

        // حذف کالایدر
        Destroy(rangeVisualizer.GetComponent<Collider>());

        // تنظیم متریال
        if (rangeMaterial != null)
        {
            rangeVisualizer.GetComponent<Renderer>().material = rangeMaterial;
        }
        else
        {
            // ایجاد متریال پیش‌فرض
            Material defaultMaterial = new Material(Shader.Find("Standard"));
         //   defaultMaterial.color = new Color(1, 0, 0, 0.2f); // قرمز با شفافیت
            rangeVisualizer.GetComponent<Renderer>().material = defaultMaterial;
        }

        // مخفی کردن اولیه
        SetVisibility(false);
    }

    void Update()
    {
        // به‌روزرسانی اندازه بر اساس محدوده تشخیص
        rangeVisualizer.transform.localScale = new Vector3(detectionRange * 2, detectionRange * 2, 1);

        // به‌روزرسانی شفافیت
        UpdateAlpha();
    }

    void UpdateAlpha()
    {
        /*
        float targetAlpha = isVisible ? 0.2f : 0f;
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);

        Color currentColor = rangeVisualizer.GetComponent<Renderer>().material.color;
        currentColor.a = currentAlpha;
       rangeVisualizer.GetComponent<Renderer>().material.color = currentColor;
       */
    }

    public void SetVisibility(bool visible)
    {
        isVisible = visible;
    }

    void OnDestroy()
    {
        if (rangeVisualizer != null)
        {
            Destroy(rangeVisualizer);
        }
    }
} 
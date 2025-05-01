using UnityEngine;

public class PlayerController_ : MonoBehaviour
{
    public float moveSpeed = 5f; // سرعت حرکت
    public float rotationSpeed = 700f; // سرعت چرخش
    public float smoothRotationTime = 0.1f; // زمان نرم شدن چرخش
    public FloatingJoystick joystick; // از UI گذاشته شده در صحنه گرفته میشه
    private CharacterController controller; // کامپوننت CharacterController
    private float currentRotationVelocity; // سرعت چرخش فعلی برای نرم شدن حرکت
    private bool isMoving = false; // وضعیت حرکت بازیکن

    private void Start()
    {
        controller = GetComponent<CharacterController>(); // گرفتن کامپوننت CharacterController
    }

    private void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 moveDir = Vector3.zero;

        // جمع کردن ورودی‌های کیبورد و جوی استیک
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // دریافت ورودی از کیبورد
        horizontalInput += Input.GetAxisRaw("Horizontal");
        verticalInput += Input.GetAxisRaw("Vertical");

        // اگر جوی استیک موجود باشه، ورودی اون رو هم اضافه کن
        if (joystick != null)
        {
            horizontalInput += joystick.Horizontal;
            verticalInput += joystick.Vertical;
        }

        // محدود کردن مقدار ورودی بین -1 و 1
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);

        // ایجاد بردار حرکت
        moveDir = new Vector3(horizontalInput, 0, verticalInput);
        moveDir.Normalize(); // نرمال کردن جهت حرکت برای جلوگیری از سرعت زیاد هنگام حرکت قطری

        // بررسی وضعیت حرکت
        isMoving = moveDir.sqrMagnitude > 0.1f;

        // حرکت بازیکن با استفاده از CharacterController
        controller.Move(moveDir * moveSpeed * Time.deltaTime); // حرکت بازیکن در دنیای 3D

        // اگر بازیکن در حال حرکت باشد، بچرخ به سمت حرکت
        if (isMoving)
        {
            // محاسبه زاویه چرخش بر اساس جهت حرکت
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            
            // استفاده از SmoothDamp برای چرخش نرم‌تر
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref currentRotationVelocity,
                smoothRotationTime
            );
            
            // اعمال چرخش
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    // متد برای بررسی وضعیت حرکت
    public bool IsMoving()
    {
        return isMoving;
    }
}

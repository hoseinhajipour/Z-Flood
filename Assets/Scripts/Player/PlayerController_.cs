using UnityEngine;

public class PlayerController_ : MonoBehaviour
{
    public float moveSpeed = 5f; // سرعت حرکت
    public float rotationSpeed = 700f; // سرعت چرخش
    public FloatingJoystick joystick; // از UI گذاشته شده در صحنه گرفته میشه
    private CharacterController controller; // کامپوننت CharacterController

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
        // حرکت بازیکن با استفاده از Joystick (حرکت در جهت X و Z)
        Vector3 moveDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        moveDir.Normalize(); // نرمال کردن جهت حرکت برای جلوگیری از سرعت زیاد هنگام حرکت قطری

        // حرکت بازیکن با استفاده از CharacterController
        controller.Move(moveDir * moveSpeed * Time.deltaTime); // حرکت بازیکن در دنیای 3D

        // اگر بازیکن در حال حرکت باشد، بچرخ به سمت حرکت
        if (moveDir.sqrMagnitude > 0.1f)
        {
            // محاسبه زاویه چرخش بر اساس جهت حرکت
            float angle = Mathf.Atan2(moveDir.z, moveDir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0); // چرخش حول محور Y
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // چرخش به صورت نرم
        }
    }
}

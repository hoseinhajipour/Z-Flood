using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // مرجع به بازیکن
    public float followSpeed = 10f; // سرعت دنبال کردن دوربین
    public Vector3 offset; // فاصله دوربین از بازیکن

    private void Start()
    {
        // فاصله اولیه دوربین از بازیکن تنظیم می‌شود
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        // محاسبه موقعیت جدید دوربین با توجه به موقعیت بازیکن
        Vector3 targetPosition = player.position + offset;

        // دوربین به طور نرم و با سرعت مشخص، موقعیت جدید را دنبال می‌کند
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}

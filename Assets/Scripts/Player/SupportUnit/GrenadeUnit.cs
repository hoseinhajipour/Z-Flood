using UnityEngine;

public class GrenadeUnit : MonoBehaviour
{
    [Header("Grenade Settings")]
    public float throwInterval = 5f;
    public float throwForce = 10f;
    public float throwHeight = 2f;
    public int grenadesPerThrow = 1; // Number of grenades to throw at once

    [Header("Prefabs")]
    public GameObject grenadePrefab;

    private float nextThrowTime;

    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (Time.time >= nextThrowTime)
        {
            for (int i = 0; i < grenadesPerThrow; i++)
            {
                ThrowGrenade();
            }
            nextThrowTime = Time.time + throwInterval;
        }
    }

    void ThrowGrenade()
    {
        // Direction from this unit to player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 throwDirection = directionToPlayer + Vector3.up * throwHeight;
        throwDirection.Normalize();

        GameObject grenade = Instantiate(grenadePrefab, player.transform.position + Vector3.up, Quaternion.identity);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        rb.AddTorque(new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(-100f, 100f),
            Random.Range(-100f, 100f)
        ));
    }

    public void UpgradeGrenade(float newThrowInterval, int damageIncrease, float radiusIncrease, int newGrenadesPerThrow = 1)
    {
        throwInterval = newThrowInterval;
        grenadesPerThrow = newGrenadesPerThrow;
        GrenadeProjectile grenadeProjectile = grenadePrefab.GetComponent<GrenadeProjectile>();
        if (grenadeProjectile != null)
        {
            grenadeProjectile.damage += damageIncrease;
            grenadeProjectile.explosionRadius += radiusIncrease;
        }
    }
}

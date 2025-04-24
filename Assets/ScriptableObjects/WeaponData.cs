using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float bulletSpeed = 10f;
    public int damage = 1;
    public float range = 8f;
}

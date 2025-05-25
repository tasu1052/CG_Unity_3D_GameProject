using UnityEngine;

public class grenade : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치

    private float fireRate;
    private float nextFireTime = 0f;

    private float baseDamage;
    private float explosionRadius = 3.0f;

    public float FireRate => fireRate;
    public int fireRatenum;

    void Start()
    {
        fireRatenum = Random.Range(0, 2);
        if (fireRatenum == 0)
            fireRate = 1.5f;
        else
            fireRate = 2.0f;
        baseDamage = Random.Range(50f, 101f);

        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 150f);
        baseDamage = baseDamage * multiplier;

        Debug.Log($"[grenade] fireRate: {fireRate}, baseDamage: {baseDamage}, explosionRadius: {explosionRadius}");
    }

    void Update()
    {
        FireBullet();
    }

    public void FireBullet()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            grenadebullet bulletScript = bullet.GetComponent<grenadebullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(baseDamage);
                bulletScript.SetExplosionRadius(explosionRadius); // 🎯 radius 전달
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}

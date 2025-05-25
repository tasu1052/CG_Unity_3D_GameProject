using UnityEngine;

public class grenade : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치
    private float fireRate;
    private float nextFireTime = 0f;
    private float baseDamage;

    public float FireRate => fireRate;

    void Start()
    {
        fireRate = Random.Range(1.5f, 3.0f);
        baseDamage = Random.Range(50f, 101f);

        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 100f);
        fireRate = fireRate / multiplier;
        baseDamage = baseDamage * multiplier;

        Debug.Log($"[grenade] fireRate: {fireRate}, baseDamage: {baseDamage}");
    }

    void Update()
    {
        FireBullet();
        Debug.Log($"[grenade] fireRate: {fireRate}, baseDamage: {baseDamage}");
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
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}

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
    public void SetStats(float fireRate, float damage)
    {
        Debug.Log($"{fireRate}:{damage}");
        this.fireRate = fireRate;
        this.baseDamage = damage;
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
            SoundManager.Instance.SFXPlay("Grenade2");
        }
    }
}

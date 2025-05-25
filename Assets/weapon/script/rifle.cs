using UnityEngine;

public class rifle : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치

    private float fireRate;           // 랜덤 발사 속도
    private float nextFireTime = 0f;

    private float baseDamage;           // ⬅ 총 데미지를 고정

    public float FireRate => fireRate; // 외부 참조용 프로퍼티
    public void SetStats(float fireRate, float damage)
    {
        this.fireRate = fireRate;
        this.baseDamage = damage;
    }

    void Start()
    {
        Debug.Log($"[rifle] firerate: {fireRate}, baseDamage: {baseDamage}");
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

            riflebullet bulletScript = bullet.GetComponent<riflebullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(baseDamage);  // ⬅ 고정된 데미지를 전달
            }

            nextFireTime = Time.time + fireRate;
            SoundManager.Instance.SFXPlay("Machine_gun");
        }
    }

}

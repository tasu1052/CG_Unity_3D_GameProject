using UnityEngine;

public class rifle : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치

    private float fireRate;           // 랜덤 발사 속도
    private float nextFireTime = 0f;

    private float baseDamage;           // ⬅ 총 데미지를 고정

    public float FireRate => fireRate; // 외부 참조용 프로퍼티
    public int fireRatenum;
    public int fireDamagenum;

    void Start()
    {
        fireRatenum = Random.Range(0, 2);
        if (fireRatenum == 0)
            fireRate = 0.3f;
        else
            fireRate = 0.5f;
        baseDamage = Random.Range(20, 41);  // ⬅ 최초 1회만 데미지 설정
        fireDamagenum = Random.Range(0, 2);
        if (fireDamagenum == 0)
            baseDamage = 25f;
        else
            baseDamage = 35f;
        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 150f);
        baseDamage = baseDamage* multiplier;
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
        }
    }

}

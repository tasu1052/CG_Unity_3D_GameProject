using UnityEngine;

public class grenade : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치
    public float fireRate = 2.0f;     // 발사 속도 (초 단위)
    private float nextFireTime = 0f;

    void Update()
    {
        FireBullet();
    }

    // 발사 트리거용 메소드 (플레이어가 발사할 때 호출)
    public void FireBullet()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }
}

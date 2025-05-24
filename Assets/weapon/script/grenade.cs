using UnityEngine;

public class grenade : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치
    private float fireRate;    // 발사 속도 (초 단위)
    private float nextFireTime = 0f;

    public float FireRate => fireRate; 


    void Start()
    {
        fireRate = Random.Range(1.5f, 3.0f); // 발사 속도 랜덤 설정
        Debug.Log($"[riflebullet] firerate: {fireRate}");
    }
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

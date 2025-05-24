using UnityEngine;

public class rifle : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public Transform firePoint;       // 총구 위치

    private float fireRate;           // 랜덤 발사 속도
    private float nextFireTime = 0f;

    public float FireRate => fireRate; // 외부 참조용 프로퍼티

    void Start()
    {
        fireRate = Random.Range(0.3f, 0.6f); // 발사 속도 랜덤 설정
        Debug.Log($"[riflebullet] firerate: {fireRate}");
    }

    void Update()
    {
        FireBullet();
    }

    // 발사 트리거용 메소드
    public void FireBullet()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }
}

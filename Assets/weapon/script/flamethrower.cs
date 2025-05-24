using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public GameObject flameParticlePrefab;  // 불꽃 파티클 프리팹
    public Transform firePoint;             // 발사 위치

    private float fireRate;                 // 랜덤 발사 속도
    private float nextFireTime = 0f;

    public float FireRate => fireRate;      // 외부 접근용

    void Start()
    {
        fireRate = Random.Range(0.05f, 0.1f); // 빠른 연사 (짧은 간격)
        Debug.Log($"[flamethrower] firerate: {fireRate}");
    }

    void Update()
    {
        FireFlame();
    }

    // 불꽃 발사
    public void FireFlame()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(flameParticlePrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }
}

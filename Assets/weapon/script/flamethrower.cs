using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public GameObject flameParticlePrefab;  // 불꽃 파티클 프리팹
    public Transform firePoint;             // 발사 위치

    private float fireRate;
    private float nextFireTime = 0f;
    private float baseDamage;

    public float FireRate => fireRate;

    void Start()
    {
        fireRate = Random.Range(0.05f, 0.1f);
        baseDamage = Random.Range(5f, 15f);  // 초기 데미지 설정

        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 100f);
        fireRate = fireRate / multiplier;
        baseDamage = baseDamage * multiplier;

        Debug.Log($"[Flamethrower] fireRate: {fireRate}, baseDamage: {baseDamage}");
    }

    void Update()
    {
        FireFlame();
        Debug.Log($"[Flamethrower] fireRate: {fireRate}, baseDamage: {baseDamage}");
    }

    public void FireFlame()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject flame = Instantiate(flameParticlePrefab, firePoint.position, firePoint.rotation);
            flamethrowerbullet flameScript = flame.GetComponent<flamethrowerbullet>();
            if (flameScript != null)
            {
                flameScript.SetDamage(baseDamage);
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}

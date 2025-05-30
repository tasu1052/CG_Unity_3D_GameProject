using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public GameObject flameParticlePrefab;  // 불꽃 파티클 프리팹
    public Transform firePoint;             // 발사 위치

    public float fireRate;
    private float nextFireTime = 0f;
    public float baseDamage;

    public float FireRate => fireRate;
    public void SetStats(float fireRate, float damage)
    {
        this.fireRate = fireRate;
        this.baseDamage = damage;
    }



    void Update()
    {
        FireFlame();
    }

    public void FireFlame()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject flame = Instantiate(flameParticlePrefab, firePoint.position, firePoint.rotation);
            flamethrowerbullet flameScript = flame.GetComponent<flamethrowerbullet>();
            if (flameScript != null)
            {
                flameScript.SetDamage(baseDamage*Managers.Game.extraDamage);
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}

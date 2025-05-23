using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public GameObject firePrefab; // 불공 프리팹
    public Transform firePoint;   // 발사 위치
    public float fireInterval = 3f; // 몇 초마다 발사할지 (기본 5초)

    private float lastFireTime = 0f;

    void Update()
    {
        if (Time.time - lastFireTime >= fireInterval)
        {
            ShootFire();
            lastFireTime = Time.time;
        }
    }

    void ShootFire()
    {
        GameObject fireInstance = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = fireInstance.GetComponent<Rigidbody>();
    }
}

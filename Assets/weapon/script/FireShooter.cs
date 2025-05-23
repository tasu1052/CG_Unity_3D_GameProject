using UnityEngine;

public class FireShooter : MonoBehaviour
{
    public GameObject firePrefab; // 불공 프리팹
    public Transform firePoint;   // 발사 위치
    public float fireForce = 10f;

    public void ShootFire()
    {
        Debug.Log("🔥 ShootFire 호출됨!");
        GameObject fireInstance = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = fireInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * fireForce);  // 앞으로 힘 줌
        }
    }
}

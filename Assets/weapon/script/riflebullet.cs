using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10; // 데미지

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 발사 방향으로 이동
        rb.velocity = transform.forward * speed;

        // 일정 시간 후 사라짐
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // EnemyHealth 스크립트를 가진 적이 있다면 데미지 적용
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // 총알 제거
        Destroy(gameObject);
    }
}

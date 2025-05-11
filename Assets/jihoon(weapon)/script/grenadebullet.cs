using UnityEngine;

public class grenadebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float explosionRadius = 3f;
    public float damage = 50f;
    public LayerMask enemyLayer;  // Enemy 레이어만 탐지

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 폭발 로직 실행
        Explode();

        // 그레네이드는 충돌 시 바로 사라짐
        Destroy(gameObject);
    }

    void Explode()
    {
        // 폭발 범위 내 콜라이더 탐색
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider hit in hitColliders)
        {
            // Enemy에게 데미지를 주는 로직 (예: EnemyHealth 컴포넌트가 있다고 가정)
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // (선택) 폭발 파티클 이펙트 생성
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }
}

using UnityEngine;

public class grenadebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float explosionRadius = 3f;
    public float damage = 50f;
    public float rotateSpeed = 5f;
    public LayerMask enemyLayer;

    private Rigidbody rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 유도 이동 시 물리력 제거
        Destroy(gameObject, lifeTime);
        FindClosestEnemy();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 타겟 방향으로 회전하며 이동
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.fixedDeltaTime, 0.0f);

        transform.position += newDir * speed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // 파티클 이펙트 추가 가능
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy"); // 적 오브젝트는 "enemy" 태그 필요
        float closestDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                target = enemy.transform;
            }
        }
    }
}

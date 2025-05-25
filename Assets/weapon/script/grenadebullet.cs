using UnityEngine;

public class grenadebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public LayerMask enemyLayer;

    public GameObject explosionEffectPrefab;

    private Rigidbody rb;
    private bool hasExploded = false;

    private float damage;
    private float explosionRadius;

    public float Damage => damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetExplosionRadius(float radius)
    {
        explosionRadius = radius;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        hasExploded = true;
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        // 폭발 이펙트
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        // 주변 적에게 피해
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // ✅ 폭발 사운드 (SoundManager 이용)
        SoundManager.Instance.SFXPlay("Grenade2"); // clipList에 "Explosion" 클립이 있어야 함
    }
}

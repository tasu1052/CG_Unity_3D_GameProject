using UnityEngine;

public class grenadebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float explosionRadius = 3f;
    public LayerMask enemyLayer;

    public AudioClip explosionSound;
    public GameObject explosionEffectPrefab;

    private Rigidbody rb;
    private bool hasExploded = false;

    public float Damage { get; private set; }

    void Start()
    {
        // ✅ 1. 데미지 설정
        float baseDamage = Random.Range(50f, 101f);

        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 100f);
        Damage = baseDamage; // * multiplier;

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
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
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
    }
}

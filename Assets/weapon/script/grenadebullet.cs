using UnityEngine;

public class grenadebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float explosionRadius = 3f;
    public float rotateSpeed = 5f;
    public LayerMask enemyLayer;

    public AudioClip explosionSound;
    public GameObject explosionEffectPrefab;

    private AudioSource audioSource;
    private Rigidbody rb;
    private Transform target;
    private bool hasExploded = false;

    public float Damage { get; private set; } 

    void Start()
    {
        // ✅ 1. 기본 데미지를 랜덤으로 설정
        float baseDamage = Random.Range(50f, 101f);

        // ✅ 2. 경과 시간에 따라 데미지 증가
        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }
        else
        {
            Debug.LogWarning("[grenadebullet] TimeManager.Instance is null! Setting elapsedTime = 0");
        }

        float multiplier = 1f + (elapsedTime / 50f);
        Damage = baseDamage;// * multiplier;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(gameObject, lifeTime);
        FindClosestEnemy();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = explosionSound;
        audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Time.fixedDeltaTime, 0.0f);

        transform.position += newDir * speed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        Explode();
        hasExploded = true;
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
                enemy.TakeDamage(Damage); // ✅ 데미지 적용 시 Damage 프로퍼티 사용
            }
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
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

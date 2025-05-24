using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    public AudioClip shootSound; // ✅ 발사 사운드 연결
    private AudioSource audioSource;

    private int damage;
    private Rigidbody rb;

    public int Damage => damage;

    void Start()
    {
        int baseDamage = Random.Range(20, 41); // 20~40

        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }
        else
        {
            Debug.LogWarning("[riflebullet] TimeManager.Instance is null! Setting elapsedTime = 0");
        }

        float multiplier = 1+ (elapsedTime / 100f);
        damage = Mathf.RoundToInt(baseDamage * multiplier);
        Debug.LogWarning("rifle damage : " + damage);

        // ✅ Rigidbody 초기화
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // ✅ 사운드 재생
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shootSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드
        audioSource.volume = 0.8f;
        audioSource.Play();

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

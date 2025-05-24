using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

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

        float multiplier = elapsedTime / 50f;
        damage = baseDamage; // Mathf.RoundToInt(baseDamage * multiplier);

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
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

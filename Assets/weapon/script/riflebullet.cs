using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float rotateSpeed = 5f;

    private int damage;
    private Rigidbody rb;
    private Transform target;

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
    damage = baseDamage;//Mathf.RoundToInt(baseDamage * multiplier);


    rb = GetComponent<Rigidbody>();
    rb.isKinematic = true;
    Destroy(gameObject, lifeTime);
    FindClosestEnemy();
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
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
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

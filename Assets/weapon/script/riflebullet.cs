using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public int damage = 10;
    public float rotateSpeed = 5f;

    private Rigidbody rb;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(gameObject, lifeTime);
        FindClosestEnemy();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // 유도 방향 계산
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

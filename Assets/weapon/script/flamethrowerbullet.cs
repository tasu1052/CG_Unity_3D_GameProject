using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;              // 데미지를 줄 반경
    public float lifeTime = 0.5f;         // 생존 시간
    public int minDamage = 5;
    public int maxDamage = 15;

    void Start()
    {
        int damage = Random.Range(minDamage, maxDamage + 1);
        DealAreaDamage(damage);
        Destroy(gameObject, lifeTime);
    }

    void DealAreaDamage(int damage)
    {
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        float radius = range;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("enemy"))
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    // 시각화용 (에디터에서 범위 확인)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Gizmos.DrawWireSphere(center, range);
    }
}

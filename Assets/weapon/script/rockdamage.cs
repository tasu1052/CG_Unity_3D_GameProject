using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float damageAmount = 100.0f;
    public float explosionRadius = 10f;     // ✅ 추가: 광역 공격 반경

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"충돌 감지됨: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Player"))
            return;

        // ✅ 광역 데미지 처리
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("enemy")) continue;

            Debug.Log($"Enemy에 맞음! 데미지 적용 대상: {hit.name}");

            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
                enemyHealth = hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Debug.Log("TakeDamage 호출!");
                enemyHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("EnemyHealth 컴포넌트를 찾을 수 없음!");
            }
        }

        Destroy(gameObject);
    }
}

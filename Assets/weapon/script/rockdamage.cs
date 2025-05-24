using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float baseDamage = 100.0f;           // ✅ 기본 데미지
    public float explosionRadius = 7f;         // 광역 공격 반경
    private float damageAmount;                 // 실제 데미지

    private void Start()
    {
        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }
        else
        {
            Debug.LogWarning("[FireDamage] TimeManager.Instance is null! Setting elapsedTime = 0");
        }

        float multiplier = 1f + (elapsedTime / 100f); // 경과 50초마다 데미지 2배
        damageAmount = baseDamage * multiplier;
        Debug.Log("fire damage: " + damageAmount);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
            return;

        // 광역 데미지 처리
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("enemy")) continue;



            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>() ?? hit.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }

        Destroy(gameObject);
    }
}

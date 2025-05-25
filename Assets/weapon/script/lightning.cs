using UnityEngine;

public class lightning : MonoBehaviour
{
    public float baseDamage = 300f;
    public float range = 5f;
    public float radius = 2f;
    public float lifetime = 2f;

    void Start()
    {
        // ✅ 사운드 매니저를 통해 번개 사운드 재생
        SoundManager.Instance.SFXPlay("Lightning");  // 🔺 SoundManager에 "Lightning"라는 이름의 클립이 등록되어 있어야 함

        // ✅ 경과 시간 기반 데미지 계산
        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 50f); // 50초 경과 시 2배 데미지
        float damageAmount = baseDamage * multiplier;
        Debug.Log("lightning damage: " + damageAmount);

        DealFrontDamage(damageAmount);
        Destroy(gameObject, lifetime);
    }

    void DealFrontDamage(float damageAmount)
    {
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Collider[] hits = Physics.OverlapSphere(center, radius);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag("enemy")) continue;

            Vector3 toTarget = (hit.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);

            if (dot > 0.5f)
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>() ?? hit.GetComponentInParent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Gizmos.DrawWireSphere(center, radius);
    }
}

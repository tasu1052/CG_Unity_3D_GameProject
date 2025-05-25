using UnityEngine;

public class lightning : MonoBehaviour
{
    public float baseDamage = 300f;
    public float range = 5f;
    public float radius = 2f;
    public float lifetime = 2f;

    void Start()
    {
        // ✅ EnemySpawner에서 현재 레벨을 가져옴
        int currentLevel = 1; // 기본값
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            currentLevel = spawner.currentLevel;
        }

        float damageAmount = baseDamage + (10* currentLevel); 
        // ✅ 사운드 매니저를 통해 번개 사운드 재생
        SoundManager.Instance.SFXPlay("Lightning");  // 🔺 SoundManager에 "Lightning"라는 이름의 클립이 등록되어 있어야 함

        // ✅ 경과 시간 기반 데미지 계산

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

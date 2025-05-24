using UnityEngine;

public class lightning : MonoBehaviour
{
    public float baseDamage = 300f;     // ✅ 고정 데미지
    public float range = 5f;
    public float radius = 2f;
    public float lifetime = 2f;

    public AudioClip lightningSound;
    private AudioSource audioSource;

    private float damageAmount;

    void Start()
    {
        // ✅ AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = lightningSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.Play();

        // ✅ 시간에 따른 데미지 증가
        float elapsedTime = 0f;
        if (TimeManager.Instance != null)
        {
            elapsedTime = TimeManager.Instance.GetElapsedTime();
        }

        float multiplier = 1f + (elapsedTime / 50f); // 경과 50초에 데미지 2배
        damageAmount = baseDamage * multiplier;
        Debug.Log("lightning damage: " + damageAmount);

        DealFrontDamage();
        Destroy(gameObject, lifetime);
    }

    void DealFrontDamage()
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

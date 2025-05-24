using UnityEngine;

public class lightning : MonoBehaviour
{
    public float damageAmount = 300f;
    public float range = 5f; // 전방 범위
    public float radius = 2f; // 원형 데미지 범위
    public float lifetime = 2f;

    public AudioClip lightningSound; // ✅ 사운드 파일 연결
    private AudioSource audioSource;

    void Start()
    {
        // ✅ AudioSource 생성 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = lightningSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드
        audioSource.volume = 1f;
        audioSource.Play();

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

            if (dot > 0.5f) // 전방 60도
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

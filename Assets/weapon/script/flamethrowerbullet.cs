using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;
    public float lifeTime = 0.5f;
    public int minDamage = 5;
    public int maxDamage = 15;

    public AudioClip flameSound; // 🔊 화염 사운드
    private AudioSource audioSource;

    void Start()
    {
        // 🔊 AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = flameSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드
        audioSource.volume = 0.7f;

        audioSource.Play(); // 🔊 재생

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Gizmos.DrawWireSphere(center, range);
    }
}

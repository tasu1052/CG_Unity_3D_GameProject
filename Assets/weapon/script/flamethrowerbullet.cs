using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;
    public float lifeTime = 0.3f;
    public AudioClip flameSound;

    private AudioSource audioSource;
    private float damage;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void Start()
    {
        // 🔊 사운드 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = flameSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 0.7f;
        audioSource.Play();

        DealAreaDamage();
        Destroy(gameObject, lifeTime);
    }

    void DealAreaDamage()
    {
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(center, range);
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

using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;
    public float lifeTime = 0.3f;
    public AudioClip flameSound; // 🔊 화염 사운드

    private AudioSource audioSource;
    private int damage; // ✅ 한 번만 랜덤 지정해서 참조

    void Start()
    {
        damage = Random.Range(5, 16); // ✅ 5~15 랜덤 데미지 지정

        // 🔊 AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = flameSound;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드
        audioSource.volume = 0.7f;
        audioSource.Play();

        DealAreaDamage(); // ✅ damage 변수 참조
        Destroy(gameObject, lifeTime);
    }

    void DealAreaDamage()
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
                    enemy.TakeDamage(damage); // ✅ 지정된 damage 사용
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

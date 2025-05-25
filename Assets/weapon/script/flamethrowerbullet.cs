using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;
    public float lifeTime = 0.3f;

    private float damage;

    public float Damage => damage;

    void Start()
    {
        // 🔊 사운드 재생: 여기서 재생하면 이 총알이 생성된 경우에만 실행됨

        DealAreaDamage();
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void DealAreaDamage()
    {
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(center, range);

        bool hasHitEnemy = false;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("enemy"))
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    hasHitEnemy = true;
                }
            }
        }
        if (hasHitEnemy)
        {
            SoundManager.Instance.SFXPlay("FireThrowerSound");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Gizmos.DrawWireSphere(center, range);
    }
}

using UnityEngine;

public class flamethrowerbullet : MonoBehaviour
{
    public float range = 3f;
    public float lifeTime = 0.3f;

    private float damage;

    public float Damage => damage;
    private float flamesound;

    void Start()
    {

        DealAreaDamage();
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
    private static float lastSoundTime = -999f;
    private static float soundCooldown = 0.2f;
    void DealAreaDamage()
    {
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(center, range);

        //bool hasHitEnemy = false;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("enemy"))
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    //hasHitEnemy = true;
                }
            }
        }
        flamesound = Random.Range(0, 8);
        if (flamesound == 1)
        { 
            SoundManager.Instance.SFXPlay("FireThrowerSound");
        }        /*if (hasHitEnemy && Time.time - lastSoundTime > soundCooldown)
        {
            SoundManager.Instance.SFXPlay("FireThrowerSound");
            lastSoundTime = Time.time;
        }*/
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * range * 0.5f;
        Gizmos.DrawWireSphere(center, range);
    }
}

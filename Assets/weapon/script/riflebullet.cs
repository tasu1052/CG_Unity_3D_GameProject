using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
<<<<<<< Updated upstream

    public AudioClip shootSound; // ✅ 발사 사운드 연결

=======
>>>>>>> Stashed changes
    private float damage;
    private Rigidbody rb;

    public float Damage => damage;

    void Start()
    {
        // Rigidbody 설정
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

<<<<<<< Updated upstream
=======
        // ✅ 발사 사운드 재생 (SoundManager 사용)
        SoundManager.Instance.SFXPlay("Machine_gun");

        // 총알 수명 설정
>>>>>>> Stashed changes
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
<<<<<<< Updated upstream
        SoundManager.Instance.SFXPlay("machine_gun");
=======
    }
>>>>>>> Stashed changes
}

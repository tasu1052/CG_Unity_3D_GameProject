using UnityEngine;

public class riflebullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    public AudioClip shootSound; // ✅ 발사 사운드 연결

    private float damage;
    private Rigidbody rb;

    public float Damage => damage;

    void Start()
    {
        //Debug.Log("rifle damage : " + damage);

        // ✅ Rigidbody 초기화
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

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
        SoundManager.Instance.SFXPlay("Machine_gun");
}
}



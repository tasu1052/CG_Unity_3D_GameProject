using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public float baseHealth = 100f;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Initialize(int level)
    {
        health = baseHealth + level * 20f; // 레벨당 체력 +20
    }

    public void TakeDamage(float amount)
    {

        //Debug.Log($"[EnemyHealth] 데미지 받음: {amount}");
        health -= amount;
        //Debug.Log($"[EnemyHealth] 현재 체력: {health}");
        if (health <= 0f)
        {
            //Debug.Log("[EnemyHealth] 사망 처리 실행");
            Die();
        }
    }

    void Die()
    {
        //Debug.Log("[EnemyHealth] Destroy 호출됨");
        if (KillManager.Instance != null)
            KillManager.Instance.AddKill();
        Destroy(gameObject);
    }
}

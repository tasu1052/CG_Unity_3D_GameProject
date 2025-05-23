using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float damageAmount = 100.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"충돌 감지됨: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Player"))
            return;

        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("Enemy에 맞음! 데미지 적용!");

            // 자식에 맞았을 경우 부모에서 찾기
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            }

            if (enemyHealth != null)
            {
                Debug.Log("TakeDamage 호출!");
                enemyHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("EnemyHealth 컴포넌트를 찾을 수 없음!");
            }
        }

        Destroy(gameObject);
    }
}

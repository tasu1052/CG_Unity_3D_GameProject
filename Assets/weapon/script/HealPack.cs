using UnityEngine;

public class HealPack : MonoBehaviour
{
    public int healAmount = 50; // 회복량

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그를 가진 오브젝트와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            // PlayerHealth 컴포넌트 가져오기
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            // 힐팩은 사용 후 파괴
            Destroy(gameObject);
        }
    }
}

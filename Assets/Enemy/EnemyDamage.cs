using UnityEngine;
using System.Collections;

public class ZombieDamage : MonoBehaviour
{
    public float damage = 10f; // 적이 플레이어에게 입히는 피해량
    private bool isDamaging = false; // 현재 데미지를 주는 중인지 여부 확인
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionStay(Collision collision)
    {
        // 충돌 대상이 플레이어인 경우
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDamaging)
            {
                StartCoroutine(DealDamageOverTime(collision.gameObject));
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 플레이어와의 충돌이 끝나면 데미지 중단
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaging = false;
        }
    }

    // 코루틴을 통해 1초 간격으로 데미지를 줌
    private IEnumerator DealDamageOverTime(GameObject player)
    {
        isDamaging = true;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        while (isDamaging && playerHealth != null && playerHealth.Hp > 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            playerHealth.GetDamage((int)damage);
            yield return new WaitForSeconds(1f); // 1초 대기
        }

        isDamaging = false;
    }
}

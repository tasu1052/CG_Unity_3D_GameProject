using UnityEngine;
using System.Collections;

public class ZombieDamage : MonoBehaviour
{
    public float damage = 10f; // ���� �÷��̾�� ������ ���ط�
    private bool isDamaging = false; // ���� �������� �ִ� ������ ���� Ȯ��

    private void OnCollisionStay(Collision collision)
    {
        // �浹 ����� �÷��̾��� ���
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
        // �÷��̾���� �浹�� ������ ������ �ߴ�
        if (collision.gameObject.CompareTag("Player"))
        {
            isDamaging = false;
        }
    }

    // �ڷ�ƾ�� ���� 1�� �������� �������� ��
    private IEnumerator DealDamageOverTime(GameObject player)
    {
        isDamaging = true;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        while (isDamaging && playerHealth != null && playerHealth.Hp > 0)
        {
            playerHealth.GetDamage((int)damage);
            yield return new WaitForSeconds(1f); // 1�� ���
        }

        isDamaging = false;
    }
}

using UnityEngine;

public class ZombieDamage : MonoBehaviour
{
    public float damage = 10f;//���� �÷��̾�� ������ ���ط�

    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� ������Ʈ�� Player �±׸� ������ �ִ� ��� �Ʒ� �ڵ� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            //�÷��̾� ������Ʈ���� PlayerHealth Ŀ���Ʈ ������
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                //PlayerHealth ������Ʈ ����� ���ض� int�� ��ȯ�Ͽ� ����
                playerHealth.GetDamage((int)damage);
            }
        }
    }
}

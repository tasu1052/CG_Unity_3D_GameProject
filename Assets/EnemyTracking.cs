using UnityEngine;

public class MonsterTracking : MonoBehaviour
{
    public Transform player;//�÷��̾��� ��ġ
    public float speed = 2f; //�� �̵��ӵ�
    public float range = 100f;//���� ����

    void Update()
    {
        //���� �÷��̾� ������ �Ÿ� ����ϴ� �ڵ�
        float distance = Vector3.Distance(transform.position, player.position);
        //�Ÿ��� ���� ���� �̳���� �÷��̾ ���� �����ӵ��� �̵��մϴ�.
        if (distance <= range)
        {
            transform.LookAt(player); //���� �÷��̾ �ִ� ������ �ٶ󺸵��� �ϴ� �ڵ�
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        //�÷��̾ ���� �����ӵ��� �̵�
        }
    }
}
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // ���� �÷��̾�
    public Vector3 offset = new Vector3(0f, 10f, -1f);  // ī�޶� ��ġ (Top View)
    void LateUpdate()
    {
        //����� �����ϸ� �����
        if (target != null)
        {
            //ī�޶� ��ġ�� ���(�÷��̾�)�� ��ġ + ���������� ����
            transform.position = target.position + offset;
            transform.LookAt(target);  // ī�޶� �׻� �÷��̾ �ٶ󺸰�
        }
    }
}
using UnityEngine;

public class BillboardTopView : MonoBehaviour
{
    //ž�ٿ� ���������� hp�ٰ� ������ �ʱ� ������ ü�¹ٰ�
    //�׻� ī�޶� ������ ���� �ֵ��� �ϱ� ���� �߰��� ��ũ��Ʈ�Դϴ�.
    //��� Update�� ���� �� ȣ��Ǵ� LateUpdate
    void LateUpdate()
    {
        //������Ʈ�� ����ȸ���� �׻� 90, 0 ,0���� ���� ��, x������ 90�� ȸ��.
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
    }
}

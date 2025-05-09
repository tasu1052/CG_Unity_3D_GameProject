using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;//�÷��̾� �̵��ӵ�
    public float turnSpeed = 5f;//�÷��̾� ȸ�� �ӵ�

    //Animator m_Animator;//�ִϸ��̼��� ����� ��� ���
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;//�̵� ���� ����

    void Start()
    {
       // m_Animator = GetComponent<Animator>();//�ִϸ����� ����
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");//�¿� ���� �Է¹���
        float vertical = Input.GetAxis("Vertical");//���� ���� �Է¹���

        m_Movement = new Vector3(horizontal, 0, vertical).normalized;
        //�Է� ���⿡ ���� �̵����� ����

        // �ִϸ��̼�  ó�� -> ����� �ּ�ó��
       // m_Animator.SetBool("isRunning", m_Movement != Vector3.zero);

        // �̵�
        transform.position += m_Movement * moveSpeed * Time.deltaTime;

        // ȸ��
        if (m_Movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_Movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }
}
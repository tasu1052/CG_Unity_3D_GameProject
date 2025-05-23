using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;//플레이어 이동속도
    public float turnSpeed = 5f;//플레이어 회전 속도

    //Animator m_Animator;//애니메이션을 사용할 경우 사용
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;//이동 방향 벡터
    Transform closestEnemy;

    void Start()
    {
       // m_Animator = GetComponent<Animator>();//애니메이터 연결
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");//좌우 방향 입력받음
        float vertical = Input.GetAxis("Vertical");//상하 방향 입력받음

        m_Movement = new Vector3(horizontal, 0, vertical).normalized;
        //입력 방향에 따라 이동방향 설정

        // 애니메이션  처리 -> 현재는 주석처리
       // m_Animator.SetBool("isRunning", m_Movement != Vector3.zero);

        // 이동
        transform.position += m_Movement * moveSpeed * Time.deltaTime;


        // 가장 가까운 적을 바라보게 회전
        FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.position - transform.position).normalized;
            direction.y = 0; // y축 회전 방지 (수평 회전만)
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }
        }
    }
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float closestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = enemy.transform;
            }
        }

        closestEnemy = nearest;
    }
}
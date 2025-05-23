using UnityEngine;

public class MonsterTracking : MonoBehaviour
{
    public Transform player;//플레이어의 위치
    public float speed = 2f; //적 이동속도
    public float range = 100f;//감지 범위
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //적과 플레이어 사이의 거리 계산하는 코드
        float distance = Vector3.Distance(transform.position, player.position);
        //거리가 감지 범위 이내라면 플래이어를 향해 일정속도로 이동합니다.
        if (distance <= range)
        {
            animator.SetBool("isMoving", true);
            transform.LookAt(player); //적이 플레이어가 있는 방향을 바라보도록 하는 코드
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            //플레이어를 향해 일정속도로 이동
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
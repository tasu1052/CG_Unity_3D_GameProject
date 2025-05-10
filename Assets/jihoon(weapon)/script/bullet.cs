using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 발사 방향: 자신의 정면 방향으로 힘을 줌
        rb.velocity = transform.forward * speed;

        // 일정 시간 뒤에 파괴
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌 처리
        Destroy(gameObject);
    }
}

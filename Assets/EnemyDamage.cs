using UnityEngine;

public class ZombieDamage : MonoBehaviour
{
    public float damage = 10f;//적이 플레이어에게 입히는 피해량

    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 오브젝트가 Player 태그를 가지고 있는 경우 아래 코드 실행
        if (collision.gameObject.CompareTag("Player"))
        {
            //플레이어 오브젝트에서 PlayerHealth 커모넌트 가져옴
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                //PlayerHealth 컴포넌트 존재시 피해랑 int로 전환하여 전달
                playerHealth.GetDamage((int)damage);
            }
        }
    }
}

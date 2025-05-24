using UnityEngine;
using TMPro;

public class SimpleCooldown : MonoBehaviour
{
    public TextMeshProUGUI cooldownText;
    public float cooldownTime = 5f;
    public FireShooter fireShooter; // 여기를 수정!

    private float currentCooldown = 0f;

    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(currentCooldown).ToString();

            if (currentCooldown <= 0)
            {
                cooldownText.text = "";
            }
        }
    }

    void UseSkill()
    {
        currentCooldown = cooldownTime;
        cooldownText.text = cooldownTime.ToString();

        if (fireShooter != null)
        {
            fireShooter.Invoke("ShootFire", 0f); // private 함수이므로 Invoke로 호출
        }
    }

    public void TriggerCooldown()
    {
        if (currentCooldown > 0)
        {
            return; // 쿨타임 시작 + 스킬 발사
        }
        UseSkill();
    }
    public bool IsReady()
    {
        return currentCooldown <= 0f;
    }

}

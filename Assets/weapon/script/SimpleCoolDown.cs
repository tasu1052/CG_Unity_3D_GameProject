using UnityEngine;
using TMPro;

public class SimpleCooldown : MonoBehaviour
{
    public TextMeshProUGUI cooldownText;
    public float cooldownTime = 5f;
    public PlayerSkill playerSkill; // ← 여기를 FireShooter 대신 PlayerSkill로

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

        // Q 키 입력 처리 (중앙에서)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TriggerCooldown();
            SoundManager.Instance.SFXPlay("meteor");
        }
    }

    void UseSkill()
    {
        currentCooldown = cooldownTime;
        cooldownText.text = cooldownTime.ToString();

        if (playerSkill != null)
        {
            playerSkill.CastSkill(); // PlayerSkill의 공개 메서드 호출
        }
    }

    public void TriggerCooldown()
    {
        if (currentCooldown > 0)
        {
            return; // 쿨타임 중이라면 무시
        }

        UseSkill();
    }

    public bool IsReady()
    {
        return currentCooldown <= 0f;
    }
}

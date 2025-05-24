using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPrefab;       // 스킬 이펙트 프리팹 (ex. 불덩이)
    public Transform skillSpawnPoint;    // 스킬 생성 위치
    public float skillCooldown = 5f;

    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q) && cooldownTimer <= 0f)
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        Instantiate(skillPrefab, skillSpawnPoint.position, skillSpawnPoint.rotation);
        cooldownTimer = skillCooldown;
    }
}

using UnityEngine;


public class PlayerSkill : MonoBehaviour
{
    public GameObject skillPrefab;
    public float skillSpeed = 10f;
    public float spawnOffset = 1.5f;
    public Transform playerTransform; // 플레이어 위치

    public void CastSkill()
    {
        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * spawnOffset;
        Quaternion rotation = Quaternion.LookRotation(playerTransform.forward);

        GameObject skill = Instantiate(skillPrefab, spawnPosition, rotation);

        Rigidbody rb = skill.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerTransform.forward * skillSpeed;
        }

        Destroy(skill, 5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawner : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    [Header("Spawn Settings")]
    public Transform player;
    public GameObject[] enemyPrefabs;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 30f;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    public int baseEnemiesPerWave = 1;
    public float levelUpInterval = 30f;

    private float levelTimer;
    public int currentLevel = 1;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public float bossSpawnInterval = 20f;   // 보스 등장 간격 (예: 60초)
    private float bossTimer = 0f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
        bossTimer += Time.deltaTime;

        if (levelTimer >= levelUpInterval)
        {
            currentLevel++;
            levelTimer = 0;
        }

        if (bossTimer >= bossSpawnInterval)
        {
            SpawnBoss();
            bossTimer = 0f; // 보스 타이머 초기화
        }

        if(levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemies()
    {
        int enemiesToSpawn = baseEnemiesPerWave + currentLevel;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        MonsterTracking tracker = enemy.GetComponent<MonsterTracking>();
        if (tracker != null)
        {
            tracker.player = player;
        }
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.Initialize(currentLevel);
    }

    void SpawnBoss()
    {
        Vector3 spawnPos = GetRandomSpawnPosition();
        GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        MonsterTracking tracker = boss.GetComponent<MonsterTracking>();
        if (tracker != null)
        {
            tracker.player = player;
        }
        EnemyHealth health = boss.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.Initialize(currentLevel * InventoryManager.Instance.nowUpgradeNumber);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * distance;

        return player.position + offset;
    }
}

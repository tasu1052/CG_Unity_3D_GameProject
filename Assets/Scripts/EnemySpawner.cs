using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform player;
    public GameObject[] enemyPrefabs;
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 30f;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;     // 시작 시 생성 간격
    public int baseEnemiesPerWave = 1;   // 초기 생성 수
    public float levelUpInterval = 30f;  // 레벨업 주기 (초)

    private float levelTimer;
    private int currentLevel = 1;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        levelTimer += Time.deltaTime;

        if (levelTimer >= levelUpInterval)
        {
            currentLevel++;
            levelTimer = 0;
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
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            0,
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ) * distance;

        Vector3 spawnPos = player.position + offset;

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

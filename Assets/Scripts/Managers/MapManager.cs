using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;     // 사용할 타일 프리팹들
    public Transform player;             // 플레이어
    public int tileRange = 1;            // 주변 타일 생성 범위 (예: 1이면 3x3)
    public float tileSize = 80f;         // 타일 한 변 크기
    public float unloadDistance = 160f;  // 너무 멀어지면 제거할 거리

    private Dictionary<Vector2Int, GameObject> tiles = new(); // 현재 활성화된 타일
    private Queue<GameObject> tilePool = new();               // 비활성화된 타일

    void Start()
    {
        Vector2Int playerCoord = WorldToCoord(player.position);
        GenerateInitialTiles(playerCoord);
    }

    void Update()
    {
        Vector2Int playerCoord = WorldToCoord(player.position);
        EnsureConnectedTiles(playerCoord);
        RemoveFarTiles(player.position);
    }

    // 초기 타일 생성
    void GenerateInitialTiles(Vector2Int center)
    {
        for (int x = -tileRange; x <= tileRange; x++)
        {
            for (int y = -tileRange; y <= tileRange; y++)
            {
                Vector2Int coord = center + new Vector2Int(x, y);
                CreateTile(coord);
            }
        }
    }

    // 플레이어 주변에 연결된 타일이 없으면 새로 생성 또는 재사용
    void EnsureConnectedTiles(Vector2Int center)
    {
        for (int x = -tileRange; x <= tileRange; x++)
        {
            for (int y = -tileRange; y <= tileRange; y++)
            {
                Vector2Int coord = center + new Vector2Int(x, y);
                if (tiles.ContainsKey(coord)) continue;

                if (IsConnected(coord))
                {
                    CreateTile(coord);
                }
            }
        }
    }

    // 해당 좌표의 인접한 4방향 중 하나라도 타일이 있으면 true
    bool IsConnected(Vector2Int coord)
    {
        return tiles.ContainsKey(coord + Vector2Int.up) ||
               tiles.ContainsKey(coord + Vector2Int.down) ||
               tiles.ContainsKey(coord + Vector2Int.left) ||
               tiles.ContainsKey(coord + Vector2Int.right);
    }

    // 타일 생성 또는 재사용
    void CreateTile(Vector2Int coord)
    {
        GameObject tile;
        if (tilePool.Count > 0)
        {
            tile = tilePool.Dequeue();
            tile.SetActive(true);
        }
        else
        {
            GameObject prefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            tile = Instantiate(prefab);
        }

        tile.transform.position = CoordToWorld(coord);
        tiles[coord] = tile;
    }

    // 일정 거리 이상 멀어진 타일 제거 (풀에 반납)
    void RemoveFarTiles(Vector3 playerPos)
    {
        List<Vector2Int> toRemove = new();

        foreach (var pair in tiles)
        {
            float dist = Vector3.Distance(CoordToWorld(pair.Key), playerPos);
            if (dist > unloadDistance)
            {
                toRemove.Add(pair.Key);
            }
        }

        foreach (var coord in toRemove)
        {
            GameObject tile = tiles[coord];
            tile.SetActive(false);
            tilePool.Enqueue(tile);
            tiles.Remove(coord);
        }
    }

    // 월드 위치 → 타일 좌표
    Vector2Int WorldToCoord(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / tileSize),
            Mathf.FloorToInt(pos.z / tileSize)
        );
    }

    // 타일 좌표 → 월드 위치
    Vector3 CoordToWorld(Vector2Int coord)
    {
        return new Vector3(coord.x * tileSize, 0, coord.y * tileSize);
    }
}

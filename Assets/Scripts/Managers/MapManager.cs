using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject[] tilePrefabs;     // 지형별 타일 프리팹 [0]grass [1]bluegrass [2]swamp
    public float tileSize = 80f;         // 타일 한 변의 길이 (월드 좌표 기준)
    public int tileRange = 1;            // 플레이어 중심으로 몇 칸까지 타일을 생성할지
    public float unloadDistance = 160f;  // 플레이어와 이 거리 이상 떨어진 타일은 비활성화

    [Header("Player Reference")]
    public Transform player;

    [Header("Nature Objects By Terrain")]
    public GameObject[] grassObjects; // for grass
    public GameObject[] blueGrassObjects; // for bluegrass
    public GameObject[] swampObjects;     // for swamp

    private Dictionary<Vector2Int, GameObject> tiles = new();          // 현재 활성화된 타일들
    private Queue<GameObject> tilePool = new();                        // 타일 풀
    private Dictionary<string, Queue<GameObject>> naturePools = new(); // 자연물 풀

    void Start()
    {
        GenerateInitialTiles(WorldToCoord(player.position));
    }

    void Update()
    {
        Vector2Int playerCoord = WorldToCoord(player.position);
        EnsureConnectedTiles(playerCoord);  // 플레이어 주변에 타일이 없으면 생성
        RemoveFarTiles(player.position);    // 멀어진 타일은 비활성화 및 풀에 반환
    }

    // 초기 중심 좌표 기준으로 주변 타일 생성
    void GenerateInitialTiles(Vector2Int center)
    {
        for (int x = -tileRange; x <= tileRange; x++)
            for (int y = -tileRange; y <= tileRange; y++)
                CreateTile(center + new Vector2Int(x, y));
    }


    //플레이어 주변 타일이 연결되어 있는지 확인하고, 없으면 생성
    void EnsureConnectedTiles(Vector2Int center)
    {
        for (int x = -tileRange; x <= tileRange; x++)
        {
            for (int y = -tileRange; y <= tileRange; y++)
            {
                Vector2Int coord = center + new Vector2Int(x, y);
                if (!tiles.ContainsKey(coord) && IsConnected(coord))
                    CreateTile(coord);
            }
        }
    }


    // 해당 좌표가 기존 타일과 연결되어 있는지 확인
    bool IsConnected(Vector2Int coord)
    {
        return tiles.ContainsKey(coord + Vector2Int.up) ||
               tiles.ContainsKey(coord + Vector2Int.down) ||
               tiles.ContainsKey(coord + Vector2Int.left) ||
               tiles.ContainsKey(coord + Vector2Int.right);
    }


    // 타일을 풀에서 꺼내거나 새로 생성하고, 위치 설정 및 자연물 배치
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

        SpawnNatureObjects(tile);   // 지형에 맞는 자연물 배치
    }


    // 해당 타일 위에 자연물 랜덤 배치
    void SpawnNatureObjects(GameObject tile)
    {
        // 타일 이름에서 clone 제거 후 지형 이름 추출
        string terrainType = tile.name.Replace("(Clone)", "").Trim(); // "tile_grass", "tile_bluegrass", "tile_swamp"

        // 지형별 오브젝트 배열 선택
        GameObject[] objectSet = terrainType switch
        {
            "tile_grass" => grassObjects,
            "tile_bluegrass" => blueGrassObjects,
            "tile_swamp" => swampObjects,
            _ => null
        };

        if (objectSet == null) return;

        // 랜덤 개수만큼 자연물 배치
        int count = Random.Range(10, 20);
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = objectSet[Random.Range(0, objectSet.Length)];
            GameObject obj = GetFromPool(prefab);

            // 타일 내부에서 랜덤 위치 지정
            Vector3 position = tile.transform.position + new Vector3(
                Random.Range(-tileSize / 2 + 5, tileSize / 2 - 5),
                0,
                Random.Range(-tileSize / 2 + 5, tileSize / 2 - 5)
            );

            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            obj.transform.parent = tile.transform;
            obj.SetActive(true);
        }
    }

    // 자연물 오브젝트 풀에서 꺼내거나 새로 생성
    GameObject GetFromPool(GameObject prefab)
    {
        string key = prefab.name;
        if (!naturePools.ContainsKey(key))
            naturePools[key] = new Queue<GameObject>();

        if (naturePools[key].Count > 0)
        {
            return naturePools[key].Dequeue();
        }
        else
        {
            return Instantiate(prefab);
        }
    }


    // 플레이어와 일정 거리 이상 떨어진 타일 제거 및 풀에 반환
    void RemoveFarTiles(Vector3 playerPos)
    {
        List<Vector2Int> toRemove = new();

        foreach (var pair in tiles)
        {
            if (Vector3.Distance(CoordToWorld(pair.Key), playerPos) > unloadDistance)
                toRemove.Add(pair.Key);
        }

        foreach (var coord in toRemove)
        {
            GameObject tile = tiles[coord];

            // 자식 자연물 오브젝트들을 풀로 반환
            foreach (Transform child in tile.transform)
            {
                string key = child.name.Replace("(Clone)", "").Trim();
                child.gameObject.SetActive(false);
                naturePools[key].Enqueue(child.gameObject);
            }

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

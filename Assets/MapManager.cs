using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject tilePrefab;   // 바닥 타일 원본
    public Transform player;        // 플레이어 위치 추적
    public int tileRange = 1;       // 얼마나 넓게 생성할지 (3x3)
    public float tileSize = 10f;    // 타일 하나의 크기

    private Dictionary<Vector2Int, GameObject> tiles = new();   // 각 타일 위치를 기억
    void Start()    // 시작할 때 3x3 타일을 생성
    {
        for(int x = -tileRange; x <= tileRange; x++)
        {
            for(int y = -tileRange; y<=tileRange; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                GameObject tile = Instantiate(tilePrefab, CoordToWorld(coord), Quaternion.identity);
                tiles[coord] = tile;    // 생성한 타일의 좌표 정보를 저장해 재배치할 수 있도록
            }
        }
    }

    void Update()   // 매 프레임마다 플레이어 위치를 확인해서 재배치
    {
        Vector2Int playerCoord = WorldToCoord(player.position);
        RecenterMap(playerCoord);
    }

    Vector3 CoordToWorld(Vector2Int coord)  // 좌표를 실제 위치로 변환
    {
        return new Vector3(coord.x*tileSize, 0, coord.y*tileSize);
    }

    Vector2Int WorldToCoord(Vector3 pos)    // 실제 위치를 좌표로 변환
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x/tileSize), Mathf.FloorToInt(pos.z/tileSize));
    }

    void RecenterMap(Vector2Int center)
    {
        Dictionary<Vector2Int, GameObject> newTiles = new();

        for(int x = -tileRange; x<=tileRange; x++)
        {
            for(int y = -tileRange; y<= tileRange; y++)
            {
                Vector2Int newCoord = center + new Vector2Int(x,y);
                if(tiles.TryGetValue(newCoord, out var tile))   // 기존 타일이 있으면 그대로 사용
                {
                    newTiles[newCoord] = tile;
                    tiles.Remove(newCoord);
                }
                else
                {
                    if(tiles.Count>0)   // 기존의 남는 타일을 재활용해서 새 위치로 이동
                    {
                        var old = tiles.First();
                        GameObject oldTile = old.Value;
                        tiles.Remove(old.Key);
                        oldTile.transform.position = CoordToWorld(newCoord);
                        newTiles[newCoord] = oldTile;
                    }
                }
            }
        }

        tiles = newTiles;
    }
}

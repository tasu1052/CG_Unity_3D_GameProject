using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    [SerializeField] public GameObject itemPrefab;
    public int index; // 아이템의 인덱스
    public int x; // 들어갈 슬롯 칸
    public int y; // 들어갈 슬롯 칸
    public int width, height;
    public Quaternion quaternion;

    public int getIndex()
    {
        return index;
    }
}

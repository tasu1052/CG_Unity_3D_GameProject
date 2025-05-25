using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    // 추상 Item 관리 

    [SerializeField] public GameObject itemPrefab;
    public GameObject itemObj;
    public int index; // 아이템의 인덱스
    public int x; // 들어갈 슬롯 칸
    public int y; // 들어갈 슬롯 칸
    public int width, height;
    public Quaternion quaternion;
    public int itemUpgradeNumber;
    public Define.ItemType itemType;
    public bool nowInInvenotry;

    public GameObject spawendObject;
    public float fireRate;
    public float damage;

    public int getIndex()
    {
        return index;
    }
}

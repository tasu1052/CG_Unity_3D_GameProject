using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager _item { get; private set; }

    private void Awake()
    {
        _item = this;
    }
    public static Item getItem(int id)
    {
        switch (id)
        {
            case 0:
                return createItem<Riffle>(id, "Prefabs/Items/Riffle");
            default:
                Debug.LogError("Unknown id");
                return null;
        }
    }

    public static T createItem<T>(int id, string prefabPath) where T : Item, new()
    {
        GameObject load_item_prefab;
        T item = new T();
        item.index = id;
        load_item_prefab = Resources.Load<GameObject>(prefabPath);
        item.itemPrefab = load_item_prefab;
        item.width = item.itemPrefab.GetComponent<isItem>().widthSize;
        item.height = item.itemPrefab.GetComponent<isItem>().heightSize;
        Debug.Log(prefabPath);
        return item;
    }

    //index 아이템 생성
    public void addItemIndex(int x)
    {
        Inventory._inventory.addItem(0, 0, getItem(x));
        Debug.Log($"Items.getItem{x}");
    }
    public void checkItemList()
    {
        List<Item> items = Inventory._inventory.items;

        foreach (Item i in items)
        {
            Debug.Log(i);
        }
    
       
    }
}

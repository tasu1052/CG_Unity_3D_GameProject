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
            case 1:
                return createItem<FireFlame>(id, "Prefabs/Items/Grande");
            case 2:
                return createItem<Launcher>(id, "Prefabs/Items/Launcher");
               
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
        // 타입별로 itemType 할당
        if (typeof(T) == typeof(Riffle))
        {
            item.itemType = Define.ItemType.Riffle;
        }
        else if (typeof(T) == typeof(Launcher))
        {
            item.itemType = Define.ItemType.Launcher;

            Launcher launcher = item as Launcher;
            //launcher.range = 10;
        }
        else if (typeof(T) == typeof(FireFlame))
        {
            item.itemType = Define.ItemType.FireFlame;
        }
        else
            Debug.LogWarning("Unknown item type for generic class.");
        



        return item;
    }

    void setLauncher(Launcher launcher)
    {

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

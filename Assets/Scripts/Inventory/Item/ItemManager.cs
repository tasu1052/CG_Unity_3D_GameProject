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
                return createItem<FireFlame>(id, "Prefabs/Items/FireFlame");
            case 2:
                return createItem<Launcher>(id, "Prefabs/Items/Launcher");
               
            default:
                Debug.LogError("Unknown id");
                return null;
        }
    }

    public static T createItem<T>(int id, string prefabPath) where T : Item, new()
    {
        InventoryManager inven = InventoryManager.Instance;
        GameObject load_item_prefab;
        T item = new T();
        item.index = id;
        load_item_prefab = Resources.Load<GameObject>(prefabPath);
        item.itemPrefab = load_item_prefab;
        // 타입별로 itemType 할당
        if (typeof(T) == typeof(Riffle))
        {
            isItem isItem = item.itemPrefab.GetComponent<isItem>();
            isItem.widthSize = Random.Range(2, 4); // 2-3
            isItem.heightSize = Random.Range(1, 3); // 1-2
            item.width = isItem.widthSize;
            item.height = isItem.heightSize;


            item.itemType = Define.ItemType.Riffle;
            if(item.width==2)
            {
                item.fireRate = 0.5f;
            }
            else { item.fireRate = 0.7f; }
            if (item.height == 1)
            {
                item.damage = 20 + inven.nowUpgradeNumber * 10;
            }
            else { item.damage = 30 + inven.nowUpgradeNumber * 10; }  
        }
        else if (typeof(T) == typeof(Launcher))
        {
            item.itemType = Define.ItemType.Launcher;

            Launcher launcher = item as Launcher;

            isItem isItem = item.itemPrefab.GetComponent<isItem>();
            isItem.widthSize = Random.Range(2, 4); // 2-3
            isItem.heightSize = isItem.widthSize;
            item.width = isItem.widthSize;
            item.height = isItem.heightSize;
            launcher.radius = 3;
          
            if (item.width == 2)
            {
                item.damage = 70+inven.nowUpgradeNumber*15;
                item.fireRate = 1.5f;
            }
            else { item.damage = 100+inven.nowUpgradeNumber*15; item.fireRate = 2.0f; }

        }
        else if (typeof(T) == typeof(FireFlame))
        {
            item.itemType = Define.ItemType.FireFlame;

            FireFlame fire = item as FireFlame;

            isItem isItem = item.itemPrefab.GetComponent<isItem>();
            isItem.widthSize = Random.Range(3, 5); // 3-4
            isItem.heightSize = Random.Range(2, 4); // 2-3
            item.width = isItem.widthSize;
            item.height = isItem.heightSize;

            fire.range = 3;
  
            if (item.width == 3)
            {
                item.fireRate = 0.05f;
            }
            else { item.fireRate = 0.1f; }
            if (item.height == 2)
            {
                item.damage = 1 + inven.nowUpgradeNumber * 1.1f;
            }
            else { item.damage = 2 + inven.nowUpgradeNumber * 1.1f; }
        }
        else
            Debug.LogWarning("Unknown item type for generic class.");


        Debug.Log($"AttachFlame 에서 값 확인 : {item.damage} : {item.fireRate}");

 

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

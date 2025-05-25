using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // 싱글톤
    public RectTransform inventoryTransform;
    public int nowUpgradeNumber = 0;
    private void Start()
    {
        Instance = this;
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
 
    }
    public void CloseInventory()
    {
        Inventory inven = Inventory._inventory;
        Time.timeScale = 1;
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
        CheckAddedItem();
        CheckOutedItem();
        inven.UpgradeItemsReset();
        inven.isUpgradeItemUsed = false;
        KillManager.Instance.canOpen = true;
    }

    public void OpenInventory()
    {
        Time.timeScale = 0;
        Inventory inven = Inventory._inventory;

        inven.UpgradeItemsList();
        nowUpgradeNumber++;
        inventoryTransform.anchoredPosition = new Vector2(0, 0); // 화면에서 보이게하기
    }


    public void CheckAddedItem()
    {
        Inventory inven = Inventory._inventory;
        foreach(Item item in inven.items)
        {
            if(item.nowInInvenotry)
            {
                inven.addAndRealzieWeapon(item);
                item.nowInInvenotry = false;
            }
        }
    }
    public void CheckOutedItem()
    {
        Inventory inven = Inventory._inventory;
        int num = 0;
        foreach (Item item in inven.getOutitems)
        {
            if (item != null)
            {
                num++;
                inven.outAndDeRealizeWeapon(item);
            }
        }
        Debug.Log(num);
        inven.getOutitems.Clear();
    }
}

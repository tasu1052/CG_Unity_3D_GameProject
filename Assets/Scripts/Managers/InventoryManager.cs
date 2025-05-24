using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // 싱글톤
    public RectTransform inventoryTransform;
    private void Start()
    {
        Instance = this;
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
    }
    public void CloseInventory()
    {
        Time.timeScale = 1;
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
        CheckAddedItem();
    }

    public void OpenInventory()
    {
        Time.timeScale = 0;
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
}

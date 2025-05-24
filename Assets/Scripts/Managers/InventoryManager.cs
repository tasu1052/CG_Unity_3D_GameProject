using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public RectTransform inventoryTransform;
    private void Start()
    {
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
    }
    public void CloseInventory()
    {
        inventoryTransform.anchoredPosition = new Vector2(0, 2000); // 화면에서 안보이게하기
    }

    public void OpenInventory()
    {
        inventoryTransform.anchoredPosition = new Vector2(0, 0); // 화면에서 보이게하기
    }
}

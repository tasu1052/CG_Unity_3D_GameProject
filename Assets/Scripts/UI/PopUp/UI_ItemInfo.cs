using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ItemInfo : UI_Popup
{
    [SerializeField]
    private TMP_Text ItemName;
    [SerializeField]
    private TMP_Text ItemType;

    private void Awake()
    {
        ItemName.text = Inventory._inventory.tmpDraggingItem.itemType.ToString();

    }

}

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
    [SerializeField]
    private TMP_Text ItemDamage;
    [SerializeField]
    private TMP_Text ItemFireRate;
    [SerializeField]
    private TMP_Text ItemAbility;

    private void Awake()
    {
        ItemName.text = Inventory._inventory.tmpDraggingItem.itemType.ToString();
        ItemDamage.text = $"공격력 : {Inventory._inventory.tmpDraggingItem.damage}";
        ItemFireRate.text = $"공격속도 : {Inventory._inventory.tmpDraggingItem.fireRate}";
        ItemAbility.text = $"능력 :  : (가로:세로) : {Inventory._inventory.tmpDraggingItem.width} : {Inventory._inventory.tmpDraggingItem.height}";
    }

}

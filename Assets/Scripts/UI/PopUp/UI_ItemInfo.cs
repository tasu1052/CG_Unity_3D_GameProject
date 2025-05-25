using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ItemInfo : UI_Popup
{
    [SerializeField]
    private TMP_Text ItemName;
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
        if (Inventory._inventory.tmpDraggingItem.itemType == Define.ItemType.FireFlame)
        {
            ItemAbility.text = $"능력 :  : " + "낮은 데미지를 가지지만 광역 공격이 가능하다. 크기가 크다." +
                $"\n(가로:세로) : {Inventory._inventory.tmpDraggingItem.width} : {Inventory._inventory.tmpDraggingItem.height}";
        }
        else if (Inventory._inventory.tmpDraggingItem.itemType == Define.ItemType.Riffle)
        {
            ItemAbility.text = $"능력 :  : " + " 높은 공격력을 가졌지만 단일 타겟이다. 크기가 작다." +
                $"\n(가로:세로) : {Inventory._inventory.tmpDraggingItem.width} : {Inventory._inventory.tmpDraggingItem.height}";
        }
        else if (Inventory._inventory.tmpDraggingItem.itemType == Define.ItemType.Launcher)
        { 
            ItemAbility.text = $"능력 :  :" + " 근접해 있는 적에게 강력한 데미지를 준다." +
                $"\n (가로:세로) : {Inventory._inventory.tmpDraggingItem.width} : {Inventory._inventory.tmpDraggingItem.height}"; 
        }
    }


}

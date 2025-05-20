using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class isItem : MonoBehaviour
{
    private string itemName; // 이름
    public int widthSize; // 물건의 가로길이
    public int heightSize; // 물건의 세로 길이
    public string ability; // 물건의 능력 설명
    // 저장되는 인벤토리내 아이템 위치
    public int storageSlotX;
    public int storageSlotY;
    public Quaternion quaternion;

    public void setSize()
    {
        int slotSize = Define.SlotData.slotSize;
        GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize * widthSize, slotSize * heightSize);
    }

}

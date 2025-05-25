using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class isItem : MonoBehaviour
{
    // 실체 isItem
    private string itemName; // 이름
    public int widthSize; // 물건의 가로길이
    public int heightSize; // 물건의 세로 길이
    public string ability; // 물건의 능력 설명
    // 저장되는 인벤토리내 아이템 위치
    public int storageSlotX;
    public int storageSlotY;
    public Quaternion quaternion;
    public Item item;

    public void setSize()
    {
        int slotSize = Define.SlotData.slotSize;

        // 회전 각도 확인
        float angleZ = quaternion.eulerAngles.z % 360;

        int w = widthSize;
        int h = heightSize;

 

        // 회전이 90도 또는 270도일 경우 width/height 바꿔서 적용
        if (Mathf.Approximately(angleZ,90f)|| Mathf.Approximately(angleZ,270f))
        {
            Debug.Log("확인");
            int temp = w;
            w = h;
            h = temp;
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize * w, slotSize * h);

        Debug.Log($"[setSize] angleZ: {angleZ} | sizeDelta: ({w}, {h})");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Slot : MonoBehaviour
{
    public Item item; // 슬롯이 어떤 아이템을 가지고잇는지
    public bool occupied = false; // 칸이 찼는지 확인하는 변수
    public RectTransform position; // 이 칸의 위치
    public int slotPositionX, slotPositionY; // 이 칸의 인벤토리 칸 위치

    private Image image;

    public void setHighLight()
    {
        image = GetComponent<Image>();
        image.color = new Color(243f / 255f, 237f / 255f, 35f / 255f, 255f / 255f); // RGB는 0~1값만 받으므로, 255로 나눠주어야함.
    }
    public void offHighLight()
    {
        image = GetComponent<Image>();
        image.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
    }
}

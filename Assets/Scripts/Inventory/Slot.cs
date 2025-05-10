using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot : MonoBehaviour
{
    public bool occupied = false; // 칸이 찼는지 확인하는 변수
    public RectTransform position; // 이 칸의 위치
    public int slotPositionX, slotPositionY; // 이 칸의 인벤토리 칸 위치
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory _inventory { get; private set; }

    public Slot[,] inventory_slot_list;

    [Header("Prefab Objects")]
    [SerializeField] private GameObject _slotUIPrefab; // 복사할 슬롯 하나

    [Header("Slots")]
    [SerializeField] public float slotwidthRect = 0;  // 하나의 슬롯의 가로 크기
    [SerializeField] public float slotheightRect = 0; // 하나의 슬롯의 세로 크기
    [SerializeField] private int slotwidthSize;       // 가로 슬롯 개수
    [SerializeField] private int slotheightSize;      // 세로 슬롯 개수

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        InitSlots(slotwidthSize, slotheightSize);
    }

    #region Slots
    private void InitSlots(int slotWidthSize, int slotHeightSize)
    {
        inventory_slot_list = new Slot[slotWidthSize, slotHeightSize];

        // 슬롯 위치 기준점 추출
        RectTransform setXYrt = _slotUIPrefab.GetComponent<RectTransform>();
        float first_slot_x = setXYrt.anchoredPosition.x;
        float first_slot_y = setXYrt.anchoredPosition.y;

        for (int y = 0; y < slotHeightSize; y++)
        {
            for (int x = 0; x < slotWidthSize; x++)
            {
                GameObject cloneslot = CloneSlot(); // Instantiate

                RectTransform clonert = cloneslot.GetComponent<RectTransform>();
                clonert.anchoredPosition = new Vector3(
                    first_slot_x + slotwidthRect * x,
                    first_slot_y - slotheightRect * y
                );

                cloneslot.SetActive(true);

                inventory_slot_list[x, y] = cloneslot.GetComponent<Slot>();
                inventory_slot_list[x, y].position = clonert;

                cloneslot.name = $"Inventory Slot [{x}],[{y}]";
                inventory_slot_list[x, y].slotPositionX = x;
                inventory_slot_list[x, y].slotPositionY = y;
            }
        }

        GameObject CloneSlot()
        {
            // Instantiate WITHOUT world position to keep local UI layout intact
            GameObject parent = GameObject.Find("InventorySlots");
            GameObject cloneSlotPrefab = Instantiate(_slotUIPrefab, parent.transform);
            return cloneSlotPrefab;
        }
    }
    #endregion
}

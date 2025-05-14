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

    // --------------Item----------- //
    List<Item> items = new List<Item>();

    // --------------Temp------------ //
    private RectTransform draggingItemRectTransform;
    private void Start()
    {
        Init();
        _inventory = this;
    }


    public void testAddItem()
    {
        ItemManager._item.addItemIndex(0);
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

    #region Item
    public bool addItem(int x,int y,Item item)
    {
        if (tryAddItem(x, y, item))
        {
            Debug.Log($"addItem([{x}],[{y}],{item})");
            item.x = x;
            item.y = y;
            for (int sY = 0; sY < item.height; sY++)
            {
                for (int sX = 0; sX < item.width; sX++)
                {
                    this.inventory_slot_list[sX + x, sY + y].occupied = true;
                }
            }

            GameObject saving_item_object = Instantiate(item.itemPrefab, this.inventory_slot_list[x, y].position.anchoredPosition, Quaternion.identity, GameObject.Find("InventorySlots").transform); // 캔버스에 구현, 이때 Find를 선택하는 것보다 그냥 캔버스에 바로 구현하는게 성능이 좋다. 시간나면 수정 요망. 
            isItem saving_item_object_isitem = saving_item_object.GetComponent<isItem>(); // isItem에 정보저장
     

            RectTransform saving_item_object_RectTransform = saving_item_object.GetComponent<RectTransform>(); // 위치조정할 RectTransform 불러오기
            saving_item_object_RectTransform.anchoredPosition = new Vector3(this.inventory_slot_list[x, y].position.anchoredPosition.x + slotwidthRect * 0.5f * (item.width - 1), this.inventory_slot_list[x, y].position.anchoredPosition.y - slotheightRect * 0.5f * (item.height - 1), 0); //위치설정
            // 무언가 이상할떈 Pivot등을 확인

            if (draggingItemRectTransform)
            {
                //dragging_item_isitem.rotation = dragging_item_RectTransform.eulerAngles.z;//회전반영
                saving_item_object.GetComponent<RectTransform>().rotation = item.quaternion;
            }


            saving_item_object.SetActive(true); //구현

            items.Add(item);

            inventory_slot_list[x, y].item = item; //슬롯에 아이템 저장
            Debug.Log(inventory_slot_list[x, y].item); //

            return true; // 아이템 add성공
        }
        else { Debug.Log("addItem 실패");  return false; } //아이템 add실패
    }

    public bool tryAddItem(int x, int y, Item item) // 넣을 수 있는지 체크
    {
        //전체 범위를 벗어났을 경우
        if (x + item.width > slotwidthSize || y + item.height > slotheightSize)
        {
            Debug.Log($"바운더리 오류 : {x}+{item.width}>{slotwidthSize} || {y}+{item.height}>{slotheightSize} ");
            return false;
        }

        // 가로세로 체크후 slots에 occupied되어 있으면 additem 불가
        for (int heightcheck = y; heightcheck < y + item.height; heightcheck++)
        {
            for (int widthcheck = x; widthcheck < x + item.width; widthcheck++)
            {
                if (inventory_slot_list[widthcheck, heightcheck].occupied == true)
                {
                    Debug.Log("Occupied slots");
                    return false;
                }

            }

        }
        return true;
    }


    #endregion
}

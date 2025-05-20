using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public static Inventory _inventory { get; private set; }

    public Slot[,] inventorySlotList;
    public List<Item> inventoryItemList;

    [Header("Prefab Objects")]
    [SerializeField] private GameObject _slotUIPrefab; // 복사할 슬롯 하나

    [Header("Slots")]
    [SerializeField] public float slotwidthRect = 0;  // 하나의 슬롯의 가로 크기
    [SerializeField] public float slotheightRect = 0; // 하나의 슬롯의 세로 크기
    [SerializeField] private int slotwidthSize;       // 가로 슬롯 개수
    [SerializeField] private int slotheightSize;      // 세로 슬롯 개수

    [Header("Connected Objects")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform mousePointer;
    [SerializeField] private RectTransform[] upgradeRects;
    // --------------Item----------- //
    List<Item> items = new List<Item>(); // 인벤토리에 있는 아이템 목록

    // --------------Temp------------ //
    private RectTransform draggingItemRectTransform;
    private Slot tmpDraggingStartSlot;
    private Item tmpDraggingItem;
    private GameObject tmpDraggingObj;
    private List<Slot> hilightSlotList = new List<Slot>();
    private isItem draggingItemisItem;
   

    // --------------RayCast---------- //
    private List<RaycastResult> _rrList;
    private GraphicRaycaster _gr;
    private PointerEventData _ped;

    private void Start()
    {
        Init();
        _inventory = this;
        test();
    }


    public void testAddItem()
    {
        ItemManager._item.addItemIndex(0);


    }
    private void Init()
    {
        InitSlots(slotwidthSize, slotheightSize);

        //Raycast관련 초기화
        _rrList = new List<RaycastResult>();
        _gr = canvas.GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(null);
    }

    #region Slots
    private void InitSlots(int slotWidthSize, int slotHeightSize)
    {
        inventorySlotList = new Slot[slotWidthSize, slotHeightSize];

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
                    first_slot_y - slotheightRect * y,
                    -10
                );

                cloneslot.SetActive(true);

                inventorySlotList[x, y] = cloneslot.GetComponent<Slot>();
                inventorySlotList[x, y].position = clonert;

                cloneslot.name = $"Inventory Slot [{x}],[{y}]";
                inventorySlotList[x, y].slotPositionX = x;
                inventorySlotList[x, y].slotPositionY = y;
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
    public bool addItem(int x, int y, Item item)
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
                    this.inventorySlotList[sX + x, sY + y].occupied = true;
                }
            }

            GameObject savingItemObject = Instantiate(item.itemPrefab, this.inventorySlotList[x, y].position.anchoredPosition, Quaternion.identity, GameObject.Find("InventorySlots").transform); // 캔버스에 구현, 이때 Find를 선택하는 것보다 그냥 캔버스에 바로 구현하는게 성능이 좋다. 시간나면 수정 요망. 
            isItem savingItemisItem = savingItemObject.GetComponent<isItem>();

            // 동기화
            savingItemisItem.setSize(); // setSize먼저해야함.
            savingItemisItem.heightSize = item.height;
            savingItemisItem.widthSize = item.width;
            savingItemisItem.quaternion = item.quaternion; // 회전값있으면 회전시키기
          
            RectTransform savingItemObjectRectTransform = savingItemObject.GetComponent<RectTransform>(); // 위치조정할 RectTransform 불러오기
            //위치설정
            savingItemObjectRectTransform.anchoredPosition = new Vector3(
                this.inventorySlotList[x, y].position.anchoredPosition.x + slotwidthRect * 0.5f * (item.width - 1),
                this.inventorySlotList[x, y].position.anchoredPosition.y - slotheightRect * 0.5f * (item.height - 1)
                );

            if (draggingItemRectTransform)
            {
                savingItemObject.GetComponent<RectTransform>().rotation = item.quaternion;
            }

            // localPosition.z를 따로 설정해야 z가 제대로 적용됨
            Vector3 fixedLocalPos = savingItemObjectRectTransform.localPosition;
            fixedLocalPos.z = 0f; // 또는 원하는 값 (-10f로 해도 OK)
            savingItemObjectRectTransform.localPosition = fixedLocalPos;

            // 무언가 이상할떈 Pivot등을 확인

            savingItemObject.SetActive(true); //구현

            // 저장위치 저장

            savingItemisItem.storageSlotX = x;
            savingItemisItem.storageSlotY = y;

            items.Add(item);

            inventorySlotList[x, y].item = item; //슬롯에 아이템 저장
            Debug.Log(inventorySlotList[x, y].item); //

            return true; // 아이템 add성공
        }
        else { Debug.Log("addItem 실패"); return false; } //아이템 add실패
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
                if (inventorySlotList[widthcheck, heightcheck].occupied == true)
                {
                    Debug.Log("Occupied slots");
                    return false;
                }

            }

        }
        return true;
    }

    private void DeleteItem(int slotX, int slotY, Slot[,] slotList) // 아이템 제거 및 초기화
    {
        Item item = slotList[slotX, slotY].item;

        for (int y = slotY; y < slotY + item.height; y++)
        {
            for (int x = slotX; x < slotX + item.width; x++)
            {
                slotList[x, y].occupied = false;
            }
        }
        slotList[slotX, slotY].item = null;
        items.Remove(item);

        if (tmpDraggingObj != null) // 현재 temp삭제
        {
            Destroy(tmpDraggingObj);
        }
    }

    private void CantAddItem()                                                 //아이템을 넣지 못할때 드래그중인 아이템 파괴 후 원래상태로 복구
    {
        // temp delete
        draggingItemRectTransform.rotation = draggingItemisItem.quaternion;
        tmpDraggingItem.quaternion = draggingItemisItem.quaternion;
        //dragging_item_RectTransform.rotation = Quaternion.Euler(0, 0, dragging_item_isitem.rotation); //회전 복구
        tmpDraggingItem.width = draggingItemisItem.widthSize;
        tmpDraggingItem.height = draggingItemisItem.heightSize;

        if (tmpDraggingStartSlot.gameObject.CompareTag("InventorySlot"))
        {//원래 저장되어있던 슬롯이 인벤토리였을경우
            DeleteItem(draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY, inventorySlotList);
            addItem(draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY, tmpDraggingItem);
        }
    

        Debug.Log("Item delete code by can't tryAddItem");
        tmpDraggingItem = null;
    }

    #endregion


    #region Dragging

    public void DraggingOn(GameObject raycastObj)
    {
        SlotHilightOff();

        Slot[,] tmpSlotList = inventorySlotList; // 일시 슬롯 저장장소
        Debug.Log(raycastObj);
        if (raycastObj.CompareTag("Item")) // 레이캐스트한 오브젝트가 아이템이라면
        {
            tmpSlotList = inventorySlotList;
            tmpDraggingObj = raycastObj;
            draggingItemRectTransform = tmpDraggingObj.GetComponent<RectTransform>();
        } else return;
        // 아이템일때만 아래코드 실행
        isItem draggingItemisItem = raycastObj.GetComponent<isItem>();

        tmpDraggingStartSlot = tmpSlotList[draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY]; // 아이템이 저장된 슬롯 위치 확인. 기준점
        tmpDraggingItem = Clone(inventorySlotList[draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY].item); // 복사하여 저장하기
        //드래그중인 아이템의 크기만큼 슬롯을 순회하면서 occupied false 만들기
        for (int sY = draggingItemisItem.storageSlotY; sY < draggingItemisItem.storageSlotY + draggingItemisItem.heightSize; sY++)
        {
            for (int sX = draggingItemisItem.storageSlotX; sX < draggingItemisItem.storageSlotX + draggingItemisItem.widthSize; sX++)
            {
                inventorySlotList[sX, sY].occupied = false; // 메인 인벤토리 슬롯 리스트 occupied 조정
            }
        }

        tmpDraggingStartSlot = tmpSlotList[draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY];

        Item Clone(Item item)
        {
            return item;
        }
    }


    public void Dragging()
    {

        SlotHilightOff();
        if (tmpDraggingObj != null)
        {

           
            for (int sX = 0; sX < tmpDraggingItem.width; sX++)
            {
                for (int sY = 0; sY < tmpDraggingItem.height; sY++)
                {
                    mousePointer.anchoredPosition = new Vector2(
                      draggingItemRectTransform.anchoredPosition.x - slotwidthRect * (0.5f * (tmpDraggingItem.width - 1)) + slotwidthRect * sX,
                      draggingItemRectTransform.anchoredPosition.y + slotheightRect * (0.5f * (tmpDraggingItem.height - 1)) - slotheightRect * sY);


                    Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, mousePointer.position);
                    GameObject tmpSlot = RaycastAndGetSecondSlot(screenPosition);

                    if (tmpSlot != null)
                    {
                        Slot tmpslot = tmpSlot.GetComponent<Slot>();
                        tmpslot.setHighLight();
                        hilightSlotList.Add(tmpslot);
                    }

                }
            }


            //R키 입력시 회전
            if (Input.GetKeyDown(KeyCode.R))
            {
                //회전적용
                draggingItemRectTransform.rotation *= Quaternion.Euler(0, 0, 90);
                tmpDraggingItem.quaternion = draggingItemRectTransform.rotation;

                //회전시 width, height변화 Item에 반영
                int temp = tmpDraggingItem.width;
                tmpDraggingItem.width = tmpDraggingItem.height;
                tmpDraggingItem.height = temp;

            }


        }
    }


    public void DraggingOff(GameObject raycasyObj)
    {
        draggingItemisItem = raycasyObj.GetComponent<isItem>();

        // ------------slotraycast ------------ //
        RectTransform rectTransformDraggingItem = raycasyObj.GetComponent<RectTransform>(); // 드래깅하던 아이템의 위치 저장

        rectTransformDraggingItem.anchoredPosition =
            new Vector2(
            rectTransformDraggingItem.anchoredPosition.x - slotwidthRect * 0.5f * (tmpDraggingItem.width - 1),
            rectTransformDraggingItem.anchoredPosition.y + slotheightRect * 0.5f * (tmpDraggingItem.height - 1)
            ); // 아이템 기준 첫번째 칸

        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransformDraggingItem.position);

        GameObject tmpSlotPrefab = RaycastAndGetSecondSlot(screenPosition); // 아이템의 위치에 있는 slot 확인

        // ------------------------------------//
       

        if (tmpSlotPrefab != null) // 슬롯이 존재한다면
        {

            Slot tmpSlotSlot = tmpSlotPrefab.GetComponent<Slot>(); // 슬롯 cs받기
            if (!tryAddItem(tmpSlotSlot.slotPositionX, tmpSlotSlot.slotPositionY, tmpDraggingItem)) // 넣을 수 없으면
            {
                CantAddItem();
                return;
            }
            else // 넣을 수 있으면
            {
                DeleteItem(draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY, inventorySlotList);
                addItem(tmpSlotSlot.slotPositionX, tmpSlotSlot.slotPositionY, tmpDraggingItem);
                Debug.Log("Add Item");
            }
        }
        else
        {
            Debug.Log("RyCastSlot error");
        }


        // 종료 및 초기화

        tmpDraggingItem = null;
        tmpDraggingObj = null;
        draggingItemisItem = null;

        foreach (Slot s in hilightSlotList)
            s.offHighLight();
        hilightSlotList.Clear();

        
    }
    #endregion


    #region Function
    public GameObject RaycastAndGetSecondSlot(Vector2 screenposition)
    {
        _rrList.Clear();

        _ped.position = screenposition;


        _gr.Raycast(_ped, _rrList);
        if (_rrList.Count <= 1)
        {
            return null;
        }

        if (_rrList[1].gameObject.CompareTag("InventorySlot") || _rrList[1].gameObject.CompareTag("StoreSlot"))
            return _rrList[1].gameObject;
        else
            return null;
    }

    private void SlotHilightOff()                   //슬롯 hilight끄기
    {
        if (hilightSlotList != null)
        {
            for (int i = 0; i < hilightSlotList.Count; i++)
            {
                hilightSlotList[i].offHighLight();
            }
            hilightSlotList.Clear();
        }
    }


    #endregion

    #region Upgrade
    private void test()
    {
        Item item = ItemManager.getItem(0);
        GameObject savingItemObject = Instantiate(item.itemPrefab, upgradeRects[0].anchoredPosition, Quaternion.identity, GameObject.Find("UpgradeSelect").transform); // 캔버스에 구현, 이때 Find를 선택하는 것보다 그냥 캔버스에 바로 구현하는게 성능이 좋다. 시간나면 수정 요망. 
        isItem savingItemisItem = savingItemObject.GetComponent<isItem>();

        // 동기화
        savingItemisItem.setSize(); // setSize먼저해야함.
        savingItemisItem.heightSize = item.height;
        savingItemisItem.widthSize = item.width;
        savingItemisItem.quaternion = item.quaternion; // 회전값있으면 회전시키기

        RectTransform savingItemObjectRectTransform = savingItemObject.GetComponent<RectTransform>(); // 위치조정할 RectTransform 불러오기
                                                                                                      //위치설정
        savingItemObjectRectTransform.anchoredPosition = upgradeRects[0].anchoredPosition;

        if (draggingItemRectTransform)
        {
            savingItemObject.GetComponent<RectTransform>().rotation = item.quaternion;
        }
        // localPosition.z를 따로 설정해야 z가 제대로 적용됨
        Vector3 fixedLocalPos = savingItemObjectRectTransform.localPosition;
        fixedLocalPos.z = 0f; // 또는 원하는 값 (-10f로 해도 OK)
        savingItemObjectRectTransform.localPosition = fixedLocalPos;

        // 무언가 이상할떈 Pivot등을 확인
        savingItemObject.SetActive(true); //구현
    }


    #endregion
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory _inventory { get; private set; }

    public Slot[,] inventorySlotList;
    public List<Item> items = new List<Item>();
    public UI_Popup opendedItemInfoPopUp;
    public List<Item> getOutitems = new List<Item>();
    public GameObject[] upgradeItemObject = new GameObject[3];

    [Header("Prefab Objects")]
    [SerializeField] private GameObject _slotUIPrefab;

    [Header("Slots")]
    [SerializeField] public float slotwidthRect = 0;
    [SerializeField] public float slotheightRect = 0;
    [SerializeField] private int slotwidthSize;
    [SerializeField] private int slotheightSize;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform garbageRectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform mousePointer;
    [SerializeField] private RectTransform[] upgradeRects;


    // Temp states
    public RectTransform draggingItemRectTransform;
    private Slot tmpDraggingStartSlot;
    public Item tmpDraggingItem;
    private GameObject tmpDraggingObj;
    public isItem draggingItemisItem;
    private List<Slot> hilightSlotList = new List<Slot>();
    private Item[] upgradeItems = new Item[3];
    public bool isUpgradeItemUsed = false;

    public bool startDragging = false;
    public bool isDragging = false;
    private bool isDraggingFromInventory = false;

    private void Start()
    {
        Init();
        _inventory = this;
    }

    private void Update()
    {
        if (isDragging)
        {
            Dragging();
        }

    }

    private void Init()
    {
        InitSlots(slotwidthSize, slotheightSize);
    }

    private void InitSlots(int width, int height)
    {
        inventorySlotList = new Slot[width, height];
        var firstPos = _slotUIPrefab.GetComponent<RectTransform>().anchoredPosition;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject slotObj = Instantiate(_slotUIPrefab, GameObject.Find("InventorySlots").transform);
                RectTransform rt = slotObj.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector3(firstPos.x + slotwidthRect * x, firstPos.y - slotheightRect * y, 0);

                slotObj.SetActive(true);
                slotObj.name = $"Inventory Slot [{x}],[{y}]";

                Slot slot = slotObj.GetComponent<Slot>();
                slot.position = rt;
                slot.slotPositionX = x;
                slot.slotPositionY = y;

                inventorySlotList[x, y] = slot;
            }
        }
    }

    #region Item Management

    public bool addItem(int x, int y, Item item)
    {
        if (!tryAddItem(x, y, item)) return false;

        item.x = x; item.y = y;

        for (int sY = 0; sY < item.height; sY++)
            for (int sX = 0; sX < item.width; sX++)
                inventorySlotList[x + sX, y + sY].occupied = true;



        GameObject itemObj = Instantiate(item.itemPrefab, inventorySlotList[x, y].position.anchoredPosition, Quaternion.identity, GameObject.Find("InventorySlots").transform);
        RectTransform itemRect = itemObj.GetComponent<RectTransform>();
        isItem itemData = itemObj.GetComponent<isItem>();
        itemData.quaternion = item.quaternion;

        itemData.heightSize = item.height;
        itemData.widthSize = item.width;
        
        itemRect.localRotation = item.quaternion;

        itemData.setSize();

       


        itemData.storageSlotX = x;
        itemData.storageSlotY = y;
        item.nowInInvenotry = true;



        itemRect.anchoredPosition = new Vector3(
            inventorySlotList[x, y].position.anchoredPosition.x + slotwidthRect * 0.5f * (item.width - 1),
            inventorySlotList[x, y].position.anchoredPosition.y - slotheightRect * 0.5f * (item.height - 1),
            0);



        // 동기화


        items.Add(item);
        inventorySlotList[x, y].item = item;
        itemObj.SetActive(true);
        return true;
    }

    public void addAndRealzieWeapon(Item item)
    {
        weaponattachmanager1 weapon = weaponattachmanager1.Instance;
        if (item.itemType == Define.ItemType.Launcher)
        {
            Launcher launcher = item as Launcher;
            item.spawendObject = weapon.AttachLauncher(launcher);
        }
        else if (item.itemType == Define.ItemType.FireFlame)
        {
            FireFlame fire = item as FireFlame;
            item.spawendObject = weapon.AttachFlame(fire);
        }
        else if (item.itemType == Define.ItemType.Riffle)
        {
            Riffle riffle = item as Riffle;
            item.spawendObject = weapon.AttachRiffle(riffle);
        }
        else if (item.itemType == Define.ItemType.CartridgeBelt)
        {
            CartridgeBelt belt = item as CartridgeBelt;
            item.spawendObject = weapon.AttachBelt(belt);
        }
    }

    public void outAndDeRealizeWeapon(Item item)
    {
        if(item.itemType==Define.ItemType.CartridgeBelt)
        {
            Managers.Game.extraDamage = 1;
        }
        Debug.Log(item.spawendObject);
        Destroy(item.spawendObject);
    }
    public bool tryAddItem(int x, int y, Item item)
    {
        if (x + item.width > slotwidthSize || y + item.height > slotheightSize)
            return false;

        for (int yCheck = y; yCheck < y + item.height; yCheck++)
            for (int xCheck = x; xCheck < x + item.width; xCheck++)
                if (inventorySlotList[xCheck, yCheck].occupied)
                    return false;

        return true;
    }

    private void DeleteItem(int x, int y)
    {
        Item item = inventorySlotList[x, y].item;

        Debug.Log($"DeleteItem ({x},{y}) -> {tmpDraggingItem.width}:{tmpDraggingItem.height}");
        inventorySlotList[x, y].item = null;
        items.Remove(item);
        if (tmpDraggingObj) Destroy(tmpDraggingObj);
    }

    private void DeleteOnly(int x, int y)
    {
        Item item = inventorySlotList[x, y].item;
        inventorySlotList[x, y].item = null;
        items.Remove(item);
        getOutitems.Add(item);
        Debug.Log(getOutitems);
        if (tmpDraggingObj) Destroy(tmpDraggingObj);
    }

    private void CantAddItem()
    {
        draggingItemRectTransform.rotation = draggingItemisItem.quaternion;
        tmpDraggingItem.quaternion = draggingItemisItem.quaternion;
        tmpDraggingItem.width = draggingItemisItem.widthSize;
        tmpDraggingItem.height = draggingItemisItem.heightSize;

        if (isDraggingFromInventory)
        {
            DeleteItem(tmpDraggingStartSlot.slotPositionX, tmpDraggingStartSlot.slotPositionY);
            addItem(draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY, tmpDraggingItem);
        }
        else
        {
            SetRectUpgradeItem();
        }

        tmpDraggingItem = null;
    }

    private void RotateItemObj()
    {
        draggingItemRectTransform.rotation *= Quaternion.Euler(0, 0, 90);
        tmpDraggingItem.quaternion = draggingItemRectTransform.rotation;


        int temp = tmpDraggingItem.width;
        tmpDraggingItem.width = tmpDraggingItem.height;
        tmpDraggingItem.height = temp;

        draggingItemisItem.widthSize = tmpDraggingItem.width;
        draggingItemisItem.heightSize = tmpDraggingItem.height;

        Debug.Log($"(width,height) : ({tmpDraggingItem.width},{tmpDraggingItem.height})");
    }

    #endregion

    #region Dragging System

    public void DraggingOn(GameObject target)
    {
        startDragging = true;
        SlotHilightOff();

        Debug.Log(target);
        tmpDraggingObj = target;
        draggingItemRectTransform = tmpDraggingObj.GetComponent<RectTransform>();
        draggingItemisItem = tmpDraggingObj.GetComponent<isItem>();


        GameObject slotObj = GetSlotUnderScreenPosition(
            RectTransformUtility.WorldToScreenPoint(null, draggingItemRectTransform.position)
        );

        if (slotObj != null)
        {
            isDraggingFromInventory = true;
            tmpDraggingStartSlot = inventorySlotList[draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY];
            tmpDraggingItem = Clone(tmpDraggingStartSlot.item);
            draggingItemisItem.item = tmpDraggingItem;

            for (int y = draggingItemisItem.storageSlotY; y < draggingItemisItem.storageSlotY + draggingItemisItem.heightSize; y++)
                for (int x = draggingItemisItem.storageSlotX; x < draggingItemisItem.storageSlotX + draggingItemisItem.widthSize; x++)
                    inventorySlotList[x, y].occupied = false;
        }
        else
        {
            isDraggingFromInventory = false;
            int index = GetUpgradeSlotUnderMouse();
            if (index >= 0) tmpDraggingItem = Clone(upgradeItems[index]);
            else Debug.LogWarning("UpgradeSlot 감지 실패");
        }


        draggingItemRectTransform.SetAsLastSibling();
        Item Clone(Item item) => item;
        isDragging = true;
    }

    public void gettmpDraggingItem(GameObject target)
    {
        tmpDraggingObj = target;
        draggingItemRectTransform = tmpDraggingObj.GetComponent<RectTransform>();
        draggingItemisItem = tmpDraggingObj.GetComponent<isItem>();
        GameObject slotObj = GetSlotUnderScreenPosition(
            RectTransformUtility.WorldToScreenPoint(null, draggingItemRectTransform.position)
        );


        if (slotObj != null)
        {
            tmpDraggingStartSlot = inventorySlotList[draggingItemisItem.storageSlotX, draggingItemisItem.storageSlotY];
            tmpDraggingItem = Clone(tmpDraggingStartSlot.item);
        }
        else
        {
            int index = GetUpgradeSlotUnderMouse();
            upgradeItemObject[index] = null;
            if (index >= 0) tmpDraggingItem = Clone(upgradeItems[index]);

        }
        Item Clone(Item item) => item;
    }

    public void Dragging()
    {
        if (tmpDraggingItem == null || draggingItemRectTransform == null) return;

        SlotHilightOff();

        // 회전 무시한 중심 기준 위치를 사용
        Vector3 itemCenter = draggingItemRectTransform.position;

        for (int x = 0; x < tmpDraggingItem.width; x++)
        {
            for (int y = 0; y < tmpDraggingItem.height; y++)
            {
                Vector2 offset = new Vector2(
                    -slotwidthRect * (tmpDraggingItem.width - 1) * 0.5f + slotwidthRect * x,
                    slotheightRect * (tmpDraggingItem.height - 1) * 0.5f - slotheightRect * y
                );

                // 회전 적용 없이 직접 보정된 화면 위치 계산
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                    null,
                    itemCenter + (Vector3)offset
                );

                GameObject slot = GetSlotUnderScreenPosition(screenPos);
                if (slot != null)
                {
                    Slot s = slot.GetComponent<Slot>();
                    s.setHighLight();
                    hilightSlotList.Add(s);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItemObj();
        }
    }



    public void DraggingOff(GameObject target)
    {
        isDragging = false;
        draggingItemisItem = target.GetComponent<isItem>();
        Vector3 itemCenter = draggingItemRectTransform.position;

        // 드래깅 기준 위치 보정 (왼쪽 상단)
        Vector2 offset = new Vector2(
                   -slotwidthRect * (tmpDraggingItem.width - 1) * 0.5f,
                   slotheightRect * (tmpDraggingItem.height - 1) * 0.5f
        );
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                  null,
                  itemCenter + (Vector3)offset
              );

        GameObject slot = GetSlotUnderScreenPosition(screenPos);

        Debug.Log(slot);

        if (slot != null)
        {
            Slot s = slot.GetComponent<Slot>();
            if (tryAddItem(s.slotPositionX, s.slotPositionY, tmpDraggingItem)) // 들어갈 수 있다면
            {
                if (isDraggingFromInventory)
                {
                    DeleteItem(tmpDraggingStartSlot.slotPositionX, tmpDraggingStartSlot.slotPositionY);
                    addItem(s.slotPositionX, s.slotPositionY, tmpDraggingItem);
                }
                else
                {
                    if (isUpgradeItemUsed) // 이미 하나가 사용되었으면 추가하지 않음
                    {
                        SetRectUpgradeItem(); // 위치 복원
                        Debug.Log("이미 Upgrade 아이템이 추가되었습니다. 더 이상 추가할 수 없습니다.");
                        ResetDraggingState(); // 드래깅 상태 초기화
                        return;
                    }

                    isUpgradeItemUsed = true; // 처음 추가되었으므로 사용 설정
                    upgradeItems[tmpDraggingItem.itemUpgradeNumber] = null;
                    addItem(s.slotPositionX, s.slotPositionY, tmpDraggingItem);
                    Destroy(tmpDraggingObj);
                }
            }
            else CantAddItem();
        }
        else if (GetIsGarbageSlot()) // 만약 쓰레기통으로 드래그되었다면
        {
            if (!isDraggingFromInventory) // Upgrade 아이템에서 왔다면
            {
                int index = tmpDraggingItem.itemUpgradeNumber;
                upgradeItems[index] = null;
                upgradeItemObject[index] = null;
            }

            if (isDraggingFromInventory)
            {
                DeleteOnly(tmpDraggingStartSlot.slotPositionX, tmpDraggingStartSlot.slotPositionY);
            }

            Destroy(tmpDraggingObj);
        }
        else
        {
            CantAddItem();

        }
        tmpDraggingItem = null;
        tmpDraggingObj = null;
        draggingItemisItem = null;
        startDragging = false;
    }

    private void ResetDraggingState()
    {
        tmpDraggingItem = null;
        tmpDraggingObj = null;
        draggingItemisItem = null;
        isDragging = false;
        startDragging = false;
    }


    #endregion

    #region Raycast Helpers (Overlay Compatible)


    private GameObject GetSlotUnderScreenPosition(Vector2 mousePos)
    {


        foreach (Slot s in inventorySlotList)
        {
            if (s == null || s.position == null) continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(s.position, mousePos, null))
                return s.gameObject;
        }

        return null;
    }

    private bool GetIsGarbageSlot()
    {
        Vector2 mousePos = Input.mousePosition;

        if (RectTransformUtility.RectangleContainsScreenPoint(garbageRectTransform, mousePos, null))
        {
            return true;
        }
        else return false;
    }


    private int GetUpgradeSlotUnderMouse()
    {
        Vector2 mousePos = Input.mousePosition;

        for (int i = 0; i < upgradeRects.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(upgradeRects[i], mousePos, null))
                return i;
        }

        return -1;
    }

    private void SlotHilightOff()
    {
        foreach (var s in hilightSlotList)
            s?.offHighLight();
        hilightSlotList.Clear();
    }

    #endregion
    public void Reset()
    {

    }

    #region Upgrade Test Items

    public void UpgradeItemsList()
    {
        for (int i = 0; i < 3; i++)
        {

            Item item = ItemManager.getItem(Random.Range(0, 4));
            GameObject itemObj = Instantiate(item.itemPrefab, upgradeRects[i].anchoredPosition, Quaternion.identity, GameObject.Find("UpgradeSelect").transform);
            isItem itemData = itemObj.GetComponent<isItem>();

            item.itemObj = itemObj;

            itemData.heightSize = item.height;
            itemData.widthSize = item.width;
           
            itemData.setSize();
            itemData.quaternion = item.quaternion;

            itemObj.GetComponent<RectTransform>().anchoredPosition = upgradeRects[i].anchoredPosition;
            itemObj.GetComponent<RectTransform>().localPosition = new Vector3(itemObj.GetComponent<RectTransform>().localPosition.x, itemObj.GetComponent<RectTransform>().localPosition.y, 0);


            itemObj.SetActive(true);
            upgradeItems[i] = item;
            item.itemUpgradeNumber = i;
        }
    }

    public void UpgradeItemsReset()
    {
        for (int i = 0; i < upgradeItems.Length; i++)
        {
            if (upgradeItems[i] != null)
            {
                if (upgradeItems[i].itemObj != null)
                {
                    Destroy(upgradeItems[i].itemObj);
                }
                upgradeItems[i] = null;
                upgradeItemObject[i] = null;
            }
        }

    }
    private void SetRectUpgradeItem()
    {
        if (tmpDraggingObj != null && tmpDraggingItem != null)
            tmpDraggingObj.GetComponent<RectTransform>().anchoredPosition = upgradeRects[tmpDraggingItem.itemUpgradeNumber].anchoredPosition;
    }

    #endregion

    
}  


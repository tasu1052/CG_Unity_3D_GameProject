using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler, IPointerEnterHandler,IPointerExitHandler
{

    private float hoverStartTime;
    private bool isHovering;
    private bool infoShown;

    private float delay = 0.5f; // 팝업 지연 시간

    // -------public--------- //
    private GameObject DraggingItemObject;
    private RectTransform DraggingItemPosition;
    // --------private-------- //
    private Canvas canvas;
    private Coroutine infoCoroutine;

    private isItem thisItem;

    private void Awake()
    {
        thisItem = GetComponent<isItem>();
        canvas = GetComponentInParent<Canvas>(); // 드래깅 캔버스받기
    }

    private void Update()
    {
        if (isHovering && !infoShown&&!Inventory._inventory.isDragging)
        {
            if (Time.unscaledTime - hoverStartTime >= delay)
            {
                ShowItemInfo();
                infoShown = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Inventory._inventory.isDragging&&(!Inventory._inventory.startDragging))
        {
            DraggingItemObject = gameObject;
            Inventory._inventory.gettmpDraggingItem(gameObject);
            isHovering = true;
            infoShown = false;
            hoverStartTime = Time.unscaledTime;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        infoShown = false;

        if (Inventory._inventory.opendedItemInfoPopUp != null)
        {
            Managers.UI.ClosePopUpUI(Inventory._inventory.opendedItemInfoPopUp);
            Inventory._inventory.opendedItemInfoPopUp = null;
        }
    }

    private void ShowItemInfo()
    {
        if (Inventory._inventory.opendedItemInfoPopUp != null) return;

        Inventory._inventory.opendedItemInfoPopUp = Managers.UI.ShowPopUpUI<UI_ItemInfo>();
        // openedItemInfoPopUp.SetItem(thisItem);

        // 아이템의 오른쪽 위치로
        RectTransform popupRect = Inventory._inventory.opendedItemInfoPopUp.GetComponent<RectTransform>();
        popupRect.anchoredPosition = Input.mousePosition;
        popupRect.anchoredPosition = popupRect.anchoredPosition + new Vector2(50f, 0); // 오른쪽으로 offset
    }

    #region DraggingFunction
    //public으로 선언해야지만 작동가능
    public void OnBeginDrag(PointerEventData eventData) //드래그 시작, 아이템 정보받기
    {
        DraggingItemObject = gameObject; // 드래그 아이템 설정
        DraggingItemPosition = DraggingItemObject.GetComponent<RectTransform>();

        Inventory._inventory.DraggingOn(gameObject);
    }
    public void OnDrag(PointerEventData eventData) //드래그중의 액션, 아이템 이동관련
    {
        // 드래깅중에 마우스 Action을 canvas로 바꿔야하기 때문에 이러한 함수를 사용한다.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 position);

        // 드래그가 시작되면 ItemInfo창끄기
        Managers.UI.ClosePopUpUI(Inventory._inventory.opendedItemInfoPopUp);


        // 받아온 position으로 이동
        DraggingItemPosition.localPosition = position;
        
    }

    public void OnEndDrag(PointerEventData eventData) //드래그 끝, 위치 찾기, add item등 
    {
        Inventory._inventory.DraggingOff(gameObject);
    }

    #endregion
}

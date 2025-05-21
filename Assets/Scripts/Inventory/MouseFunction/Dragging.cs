using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragging : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{

    // -------public--------- //
    private GameObject DraggingItemObject;
    private RectTransform DraggingItemPosition;
    // --------private-------- //
    private Canvas canvas;
    private 
    void Awake()
    {
        canvas = GetComponentInParent<Canvas>(); // 드래깅 캔버스받기
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


        // 받아온 position으로 이동
        DraggingItemPosition.localPosition = position;
        Inventory._inventory.Dragging();
        
    }
    public void OnEndDrag(PointerEventData eventData) //드래그 끝, 위치 찾기, add item등 
    {
        Inventory._inventory.DraggingOff(gameObject);
    }

    #endregion
}

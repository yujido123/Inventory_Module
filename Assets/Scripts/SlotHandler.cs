using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [HideInInspector]
    public bool hasInsert;

    [HideInInspector]
    public int prefabIndex = InventoryConst.EMPTY_SLOT_FLAG;

    public delegate void RecordBeginSlot(Transform beginDragSlot, Transform beginDragItem);
    public event RecordBeginSlot RecordBeginSlotEvent;

    //public delegate void DropInSlot(Transform transform);
    //public event DropInSlot DropInSlotEvent;

    //public InventoryPanelHandler inventoryPanelHandler;

    // 处理拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 用事件通知Panel改值
        if (transform.childCount > 0)
            RecordBeginSlotEvent(transform, transform.GetChild(0));

        // 直接改动Panel的值
        //if(transform.childCount > 0)
        //{
        //    inventoryPanelHandler.beginDragSlot = transform;
        //    inventoryPanelHandler.beginDragItem = transform.GetChild(0);
        //}
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (hasInsert)
        {
            //GetComponentInChildren<Transform>().position = Input.mousePosition;
            transform.GetChild(0).transform.position = Input.mousePosition;
        }
    }

}

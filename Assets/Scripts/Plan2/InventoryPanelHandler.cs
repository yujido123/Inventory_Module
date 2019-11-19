using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelHandler : MonoBehaviour, IDropHandler
{

    // canvas下的slot
    GameObject[] slots;

    // 记录每个slot装着哪个prefab，用index记录
    //int[] itemSlots;

    public SC_PickItem[] itemPrefabs;

    // 记录被拽起的slot和物体
    private Transform beginDragSlot;
    private Transform beginDragItem;

    // 是否已经监听slot的事件
    private bool alreadyAddBeginDragEvent;

    public Camera playerCamera;


    private void Awake()
    {

        DefaultSetting();

    }

    void DefaultSetting()
    {
        // 初始化slot
        slots = GameObject.FindGameObjectsWithTag(InventoryConst.INVENTORY_SLOT);

    }

    public void DrawInventoryUI()
    {

        for (int i = 0; i < slots.Length; i++)
        {

            GameObject theImage = slots[i];
            SlotHandler slotHandler = theImage.GetComponent<SlotHandler>();

            if(!alreadyAddBeginDragEvent)
                slotHandler.RecordBeginSlotEvent += ReceiveBeginDragEvent;

            if (slotHandler.prefabIndex > InventoryConst.EMPTY_SLOT_FLAG && !slotHandler.hasInsert)
            {
                GameObject insertObj = Instantiate(itemPrefabs[slotHandler.prefabIndex].previewObject, theImage.transform);
                insertObj.transform.localPosition = Vector3.zero;
                slotHandler.hasInsert = true;
            }

        }
        alreadyAddBeginDragEvent = true;

    }

    public void PickUpItem(int detectedItemIndex)
    {
        int slotToAdd = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<SlotHandler>().prefabIndex == InventoryConst.EMPTY_SLOT_FLAG)
            {
                slotToAdd = i;
                break;
            }
        }
        if (slotToAdd != -1)
        {
            slots[slotToAdd].GetComponent<SlotHandler>().prefabIndex = detectedItemIndex;
        }
    }



    public void OnDrop(PointerEventData eventData)
    {

        

        // 在Inventory里面拖拽
        if(RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(slots[i].GetComponent<RectTransform>(), Input.mousePosition))
                {


                    SlotHandler beginDragSlotHandler = beginDragSlot.GetComponent<SlotHandler>();
                    SlotHandler dropSlotHandler = slots[i].GetComponent<SlotHandler>();

                    // 被放置的slot自己有item
                    if (dropSlotHandler.prefabIndex > InventoryConst.EMPTY_SLOT_FLAG)
                    {
                        Transform dropSlotItem = slots[i].transform.GetChild(0);

                        // 两个slot的item调换
                        dropSlotItem.SetParent(beginDragSlot, false);
                        beginDragItem.SetParent(slots[i].transform);

                        int tempPrefabIndex = slots[i].GetComponent<SlotHandler>().prefabIndex;
                        dropSlotHandler.prefabIndex = beginDragSlot.GetComponent<SlotHandler>().prefabIndex;
                        beginDragSlotHandler.prefabIndex = tempPrefabIndex;

                    }

                    // 被放置到一个空slot
                    else
                    {
                        // 被拖物设置新parent
                        beginDragItem.SetParent(slots[i].transform);

                        beginDragSlotHandler.hasInsert = false;
                        dropSlotHandler.hasInsert = true;

                        dropSlotHandler.prefabIndex = beginDragSlot.GetComponent<SlotHandler>().prefabIndex;
                        beginDragSlotHandler.prefabIndex = InventoryConst.EMPTY_SLOT_FLAG;
                    }

                    break;
                }
            }

            // 被拖拽item若放到slot，就在slot里，若drop到slot外，就回到原slot
            beginDragItem.localPosition = Vector3.zero;
        }

        // 拖到外面的世界
        else
        {

            SlotHandler beginDragSlotHandler = beginDragSlot.GetComponent<SlotHandler>();

            // 在真实世界创建拖出的物体
            Instantiate(itemPrefabs[beginDragSlotHandler.prefabIndex], playerCamera.ScreenToWorldPoint(Input.mousePosition) + playerCamera.transform.forward, Quaternion.identity);

            // 还原slot的设置
            beginDragSlotHandler.prefabIndex = InventoryConst.EMPTY_SLOT_FLAG;
            beginDragSlotHandler.hasInsert = false;

            // 摧毁item
            Destroy(beginDragItem.gameObject);

        }


    }


    // 获取刚被拖拽瞬间的slot信息
    void ReceiveBeginDragEvent(Transform _beginDragSlot, Transform _beginDragItem)
    {
        beginDragSlot = _beginDragSlot;
        beginDragItem = _beginDragItem;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class InventoryHandler : MonoBehaviour
{
    public Texture crosshairTexture;
    public SC_CharacterController playerController;
    public InventoryPanelHandler inventoryPanel;


    public SC_PickItem[] itemPrefabs;


    // 视野正对着的物体
    private SC_PickItem detectedItem;

    bool showInventory;

    // canvas下的slot
    GameObject[] slots;

    // 记录每个slot装着哪个prefab，用index记录
    int[] itemSlots;

    // 正在注视着的prefab的index
    int detectedItemIndex;

    // 存储哪个slot开始被拽
    private SlotHandler beginDragSlot;
    // 记录被拽起来的物体
    private GameObject itemBeingDrag;


    // Start is called before the first frame update
    void Start()
    {
        DefaultSetting();
    }



    void DefaultSetting()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;




        // hide inventory panel
        inventoryPanel.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        CallInventory();
        HandlePickUpItem();
    }

    // 显示与隐藏物品栏
    void CallInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory = !showInventory;
            inventoryPanel.gameObject.SetActive(showInventory);


            // 呼叫panel 画出inventory
            inventoryPanel.DrawInventoryUI();
        }
        Cursor.visible = showInventory;
        Cursor.lockState = showInventory ? CursorLockMode.None : CursorLockMode.Locked;

        // 呼出Inventory时不给Camera转动
        playerController.canMove = !showInventory;
    }

    // 拾取物品
    void HandlePickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(detectedItem && detectedItemIndex > -1)
            {
                inventoryPanel.PickUpItem(detectedItemIndex);
                detectedItem.PickItem();
            }
        }
    }

    // 用射线找到物体
    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = playerController.playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if(Physics.Raycast(ray, out hit, 2.5f))
        {
            
            if (hit.transform.CompareTag(InventoryConst.PICKABLE_ITEM))
            {
                SC_PickItem pickItem = hit.transform.GetComponent<SC_PickItem>();
                for (int i = 0; i < itemPrefabs.Length; i++)
                {
                    if(itemPrefabs[i].itemName == pickItem.itemName)
                    {
                        detectedItem = pickItem;
                        detectedItemIndex = i;
                    }
                }

            }
            else
            {
                detectedItem = null;
            }
        }
        else
        {
            detectedItem = null;
        }
    }


    // 画出Inventory
 

    // event: 记下被拖拽的是哪个slot
    //void RecordBeginSlot(SlotHandler slotHandler, GameObject dragItem)
    //{
    //    beginDragSlot = slotHandler;

    //    itemBeingDrag = dragItem;
    //}

    //void ChangeSlotObject(Transform slotTransform)
    //{
    //    itemBeingDrag.transform.SetParent(slotTransform);
    //    itemBeingDrag.transform.localPosition = Vector3.zero;
    //}

    private void OnGUI()
    {
        DisplaySightUI();
    }

    // 显示瞄准点和捡起物品提示
    void DisplaySightUI()
    {
        if (!showInventory)
        {
            //Player crosshair
            GUI.color = detectedItem ? Color.green : Color.white;
            GUI.DrawTexture(new Rect(Screen.width / 2 - 4, Screen.height / 2 - 4, 8, 8), crosshairTexture);
            GUI.color = Color.white;

            //Pick up message
            if (detectedItem)
            {
                GUI.color = new Color(0, 0, 0, 0.84f);
                GUI.Label(new Rect(Screen.width / 2 - 75 + 1, Screen.height / 2 - 50 + 1, 150, 20), "Press 'F' to pick '" + detectedItem.itemName + "'");
                GUI.color = Color.green;
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 20), "Press 'F' to pick '" + detectedItem.itemName + "'");
            }
        }
    }


}

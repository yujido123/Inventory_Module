using UnityEngine;

public class SC_PickItem : MonoBehaviour
{
    public string itemName = "Some Item"; //Each item must have an unique name
    public Texture itemPreview;

    [HideInInspector]
    public GameObject previewObject;


    void Start()
    {
        //Change item tag
        gameObject.tag = InventoryConst.PICKABLE_ITEM;

        // assign preview object
        previewObject = transform.Find(InventoryConst.ITEM_PREVIEW).gameObject;

    }


    public void PickItem()
    {
        Destroy(gameObject);
    }
}
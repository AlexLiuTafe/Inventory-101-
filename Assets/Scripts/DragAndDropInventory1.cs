using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropInventory : MonoBehaviour
{
    #region Variables
    [Header("Inventory")]
    public bool showInv;
    public List<Item> inv = new List<Item>();
    public int slotX, slotY;
    public Rect inventorySize;
    [Header("Dragging")]
    public bool isDragging;
    public int draggedFrom;
    public Item draggedItem;
    public GameObject droppedItem;
    [Header("ToolTip")]
    public int toolTipItem;
    public bool showToolTip;
    public Rect toolTipRect;
    [Header("References and Locations")]
    public Vector2 scr;
    #endregion
    #region Clamp to Screen
    private Rect ClampToScreen(Rect r)
    {
        r.x = Mathf.Clamp(r.x, 0, Screen.width - r.width);
        r.y = Mathf.Clamp(r.y, 0, Screen.height - r.height);

        return r;
    }
    #endregion
    #region Add Item
    public void AddItem(int itemID)
    {
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].Name == null)
            {
                inv[i] = ItemData.CreateItem(itemID);
                Debug.Log("Add Item: " + inv[i].Name);
                return;
            }
        }
    }
    #endregion
    #region Drop Item
    public void DropItem()
    {
        //Drop item from inventory to the world
        droppedItem = draggedItem.ItemMesh;
        droppedItem = Instantiate(droppedItem, transform.position + transform.forward * 3, Quaternion.identity);
        droppedItem.AddComponent<Rigidbody>().useGravity = true;
        droppedItem.name = draggedItem.Name;
        droppedItem = null;
    }
    #endregion
    #region Draw Item
    void DrawItem(int windowID)
    {
        if(draggedItem.Icon != null)
        {
            GUI.DrawTexture(new Rect(0, 0, scr.x * 0.5f, scr.y * 0.5f), draggedItem.Icon);
        }
    }
    #endregion
    #region ToolTip
    #region ToolTip Content
    private string ToolTipText(int index)
    {
        //                                      new line
        string toolTipText = inv[index].Name + "\n" +
            inv[index].Description + "\nValue: " +
            inv[index].Value;

        return toolTipText;
    }
    #endregion
    #region ToolTip Window
    void DrawToolTip(int windowID)
    {
        GUI.Box(new Rect(0, 0, scr.x * 6, scr.y * 2), ToolTipText(toolTipItem));
    }
    #endregion
    #endregion
    #region Toggle Inventory
    public void ToogleInv()
    {
        if(showInv)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
    #endregion
    #region Drag Inventory
    void InventoryDrag(int windowID)
    {
        GUI.Box(new Rect(0, scr.y * 0.25f, scr.x * 6, scr.y * 0.5f), "Banner");
        GUI.Box(new Rect(0, scr.y * 4.25f, scr.x * 6, scr.y * 0.5f), "Gold Display");
        showToolTip = false;
        #region Nested For Loop
        int i = 0;
        Event e = Event.current;
        for (int y = 0; y < slotY; y++)
        {
            for (int x = 0; x < slotX; x++)
            {
                Rect slotLocation = new Rect(
                    scr.x * 0.125f + x * (scr.x * 0.75f), 
                    scr.y * 0.75f + y * (scr.y * 0.65f),
                    scr.x * 0.75f,
                    scr.y * 0.65f);
                GUI.Box(slotLocation, "");
                #region PickupItem
                #endregion
                #region Swap Item
                #endregion
                #region Place Item
                #endregion
                #region Return Item
                #endregion
                #region Draw Item Icon
                #endregion
                i++;
            }
        }
        #endregion
        #region Drag Points

        #endregion
    }
    #endregion

    #region Start

    #endregion
    #region Update

    #endregion
    #region OnGUI

    #endregion
}

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
        //Function to add the item Data to inventory list
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].Name == null)
            {
                inv[i] = ItemData.CreateItem(itemID);
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
        // inserting Icon texture 
        if (draggedItem.Icon != null)
        {
            //                                The size of the sprite when dragging the item
            GUI.DrawTexture(new Rect(0, 0, scr.x * 1, scr.y * 1), draggedItem.Icon);
        }
    }
    #endregion
    #region ToolTip
    #region ToolTip Content
    private string ToolTipText(int index)
    {
        //                            \n =   new line
        string toolTipText = inv[index].Name + "\n" +
            inv[index].Description + "\nValue: " +
            inv[index].Value;

        return toolTipText;
    }
    #endregion
    #region ToolTip Window
    void DrawToolTip(int windowID)
    {
        //Making a Text box for the toolTip (item description)
        GUI.Box(new Rect(0, 0, scr.x * 6, scr.y * 2), ToolTipText(toolTipItem));
    }
    #endregion
    #endregion
    #region Toggle Inventory
    public void ToogleInv()
    {
        if (showInv)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            showInv = false;
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            showInv = true;
        }
    }
    #endregion
    #region Drag Inventory
    void InventoryDrag(int windowID)
    {
        // Box for "Nametag" position Or any Other
        GUI.Box(new Rect(0, scr.y * 0.4f, scr.x * 6, scr.y * 0.5f), "Item");
        GUI.Box(new Rect(0, scr.y * 4.9f, scr.x * 6, scr.y * 0.5f), "Gold Display");
        showToolTip = false;
        #region Nested For Loop
        int i = 0;
        Event e = Event.current;
        for (int y = 0; y < slotY; y++)
        {
            for (int x = 0; x < slotX; x++)
            {
                //START POSITION OF THE INVENTORY SLOTS
                //                              Start position                                                    (          Slot size     )
                Rect slotLocation = new Rect(scr.x * 0.3f + x * (scr.x * 0.75f), scr.y * 1f + y * (scr.y * 0.75f), scr.x * 0.75f, scr.y * 0.75f);
                GUI.Box(slotLocation, "");
                #region PickupItem
                if (e.button == 0 && e.type == EventType.MouseDown && slotLocation.Contains(e.mousePosition)
                    && !isDragging && inv[i].Name != null && !Input.GetKey(KeyCode.LeftShift))
                {
                    draggedItem = inv[i];
                    inv[i] = new Item();
                    isDragging = true;
                    draggedFrom = i;
                    Debug.Log("Currently Dragging Your" + draggedItem.Name);
                }
                #endregion
                #region Swap Item
                //       0 =  left mouse click
                if (e.button == 0 && e.type == EventType.MouseUp && slotLocation.Contains(e.mousePosition)
                    && isDragging && inv[i].Name != null)
                {
                    Debug.Log("Swapped your" + draggedItem.Name + " With " + inv[i].Name);
                    inv[draggedFrom] = inv[i];
                    inv[i] = draggedItem;
                    draggedItem = new Item();
                    isDragging = false;
                }
                #endregion
                #region Place Item
                if (e.button == 0 && e.type == EventType.MouseUp &&
                    slotLocation.Contains(e.mousePosition) && isDragging && inv[i].Name == null)
                {
                    Debug.Log("Placing your " + draggedItem.Name);
                    inv[i] = draggedItem;
                    draggedItem = new Item();
                    isDragging = false;
                }
                #endregion
                #region Return Item
                //* NOTE IMPLEMENTED YET
                #endregion
                #region Draw Item Icon
                if (inv[i].Name != null)
                {
                    GUI.DrawTexture(slotLocation, inv[i].Icon);
                    #region Set ToolTip On Mouse Hover
                    if (slotLocation.Contains(e.mousePosition) && !isDragging && showInv)
                    {
                        toolTipItem = i;
                        showToolTip = true;
                    }
                    #endregion
                }
                #endregion
                i++;
            }
        }
        #endregion
        #region Drag Points
        // THESE ARE DRAG ZONE FOR INVENTORY WINDOW
        GUI.DragWindow(new Rect(0, 0, scr.x * 6f, scr.y * 0.25f));//Top
        GUI.DragWindow(new Rect(0, scr.y * 0.25f, scr.x * 6f, scr.y * 0.25f));//Left
        GUI.DragWindow(new Rect(scr.x * 5.5f, 0, scr.x * 6f, scr.y * 0.25f));//Right
        GUI.DragWindow(new Rect(0, scr.y * 4f, scr.x * 6f, scr.y * 0.25f));//Bottom
        #endregion
    }
    #endregion

    #region Start
    private void Start()
    {
        scr.x = Screen.width / 16;
        scr.y = Screen.height / 9;
        // THE SIZE OF OUR INVENTORY WINDOW
        inventorySize = new Rect(scr.x, scr.y, scr.x * 6.2f, scr.y * 5.5f);
        for (int i = 0; i < (slotX * slotY); i++)
        {
            inv.Add(new Item());
        }
        AddItem(0);
        AddItem(1);
        AddItem(2);
        AddItem(302);
        AddItem(401);
        AddItem(402);
    }
    #endregion
    #region Update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToogleInv();
        }
        if (scr.x != Screen.width / 16)// JUST IN CASE THE SCREEN SIZE IS DIFFERENT
        {
            scr.x = Screen.width / 16;
            scr.y = Screen.height / 9;
            inventorySize = new Rect(scr.x, scr.y, scr.x * 6f, scr.y * 4.5f);
        }
    }
    #endregion
    #region OnGUI
    private void OnGUI()
    {
        Event e = Event.current;
        #region Inventory when true
        if (showInv)
        {
            //                                  Window index set to 1 for parameter 
            inventorySize = ClampToScreen(GUI.Window(1, inventorySize, InventoryDrag, "Player Inventory"));
            #region ToolTip Display
            if (showToolTip)
            {
                //                                                                 Size of the item description box
                toolTipRect = new Rect(e.mousePosition.x + 0.01f, e.mousePosition.y + 0.01f, scr.x * 6f, scr.y * 2f);
                //     Window index 7 for parameter
                GUI.Window(7, toolTipRect, DrawToolTip, "");
            }
            #endregion
        }
        #endregion
        #region Drop Item
        if ((e.button == 0 && e.type == EventType.MouseUp && isDragging) || (isDragging && !showInv))
        {
            DropItem();
            Debug.Log("Dropped Item" + draggedItem.Name);
            draggedItem = new Item();//Assign to the drag item to null/or as new item
            isDragging = false;
        }
        #endregion
        #region Draw Item on Mouse
        if (isDragging)
        {
            if (draggedItem != null)
            {
                //                             offset position from the mouse(for the box start location)       size of the dragged item box
                Rect mouseLocation = new Rect(e.mousePosition.x + scr.x * 0.125f, e.mousePosition.y + scr.y * 0.125f, scr.x * 1f, scr.y * 1f);
                GUI.Window(72, mouseLocation, DrawItem, "");
            }
        }
        #endregion
    }
    #endregion
}

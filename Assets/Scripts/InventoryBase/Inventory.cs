using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Linear
{
    public class Inventory : MonoBehaviour
    {
        public GUISkin invSkin; //Create in Assets folder. Create -> GUI SKIN
        public GUIStyle boxStyle;
        public GUIStyle descStyle;
        #region Variables
        public List<Item> inv = new List<Item>();
        public Item selectedItem;
        public bool showInv;

        public Vector2 scr;
        public Vector2 scrollPos;

        public int money;

        public string sortType = "";
        public ItemType sortType2 = ItemType.All; //Just for shorten the code

        public Transform dropLocation;

        [System.Serializable]
        public struct equipment
        {
            public string name;
            public Transform location;
            public GameObject curItem;
        };
        public equipment[] equipmentSlots;

        
        #endregion

        private void Start()
        {

            inv.Add(ItemData.CreateItem(0));
            inv.Add(ItemData.CreateItem(1));
            inv.Add(ItemData.CreateItem(2));
            inv.Add(ItemData.CreateItem(302));
            inv.Add(ItemData.CreateItem(401));
            inv.Add(ItemData.CreateItem(402));

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) //Generate item for testing scrolling bar
            {
                inv.Add(ItemData.CreateItem(Random.Range(100, 103)));
                inv.Add(ItemData.CreateItem(Random.Range(200, 203)));
                inv.Add(ItemData.CreateItem(Random.Range(300, 303)));
                inv.Add(ItemData.CreateItem(Random.Range(400, 403)));
                inv.Add(ItemData.CreateItem(Random.Range(500, 503)));
                inv.Add(ItemData.CreateItem(Random.Range(600, 603)));
                inv.Add(ItemData.CreateItem(Random.Range(700, 703)));
                inv.Add(ItemData.CreateItem(Random.Range(800, 803)));
                inv.Add(ItemData.CreateItem(Random.Range(900, 903)));
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showInv = !showInv; //Set the show inventory to false
                if (showInv)
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    return;
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    return;
                }
            }
        }
        private void OnGUI()
        {
            if (showInv)
            {
                //Make that more efficient by only calling when you need to
                scr.x = Screen.width / 16;
                scr.y = Screen.height / 9;

                GUI.Box(new Rect(0, 0, scr.x * 10, Screen.height), "");//Background of the Panel

                //NOTE* Generate sort type button dynamicly without repeating code (SHORTEN THE CODE)
                if (GUI.Button(new Rect(4.25f * scr.x, 0, scr.x, 0.25f * scr.y),"All", boxStyle))
                {
                    sortType2 = ItemType.All;
                }
                //(int)Itemtype.All = We get the index number of "All" in the enum and convert it into int value
                for (int i = 0; i < (int)ItemType.All-1; i++)
                {
                    ItemType t = (ItemType)i;
                    //                 rearrange the button with i                        Convert the enum index(value) into string       
                    if (GUI.Button(new Rect((5.25f+i)* scr.x , 0, scr.x, 0.25f * scr.y), t.ToString(), boxStyle))
                    {
                        //          Converting an int to an enum index(value) for itemtype 
                        sortType2 = (ItemType)i;
                    }
                }
               
                //if (GUI.Button(new Rect(4.25f * scr.x, 0, scr.x, 0.25f * scr.y), "All", boxStyle))
                //{
                //    sortType = "All";
                //}
                //if (GUI.Button(new Rect(5.26f * scr.x, 0, scr.x, 0.25f * scr.y), "Food", boxStyle))
                //{
                //    sortType = "Food";
                //}
                //if (GUI.Button(new Rect(6.27f * scr.x, 0, scr.x, 0.25f * scr.y), "Armour", boxStyle))
                //{
                //    sortType = "Armour";
                //}
                //if (GUI.Button(new Rect(7.28f * scr.x, 0, scr.x, 0.25f * scr.y), "Weapon", boxStyle))
                //{
                //    sortType = "Weapon";
                //}
                //if (GUI.Button(new Rect(8.29f * scr.x, 0, scr.x, 0.25f * scr.y), "Ingredient", boxStyle))
                //{
                //    sortType = "Ingredient";
                //}


                Display();
                if (selectedItem != null)
                {
                    //Display icon and where do you want it to be displayed
                    //Calling the custom boxstyle we edit in the inspector
                    GUI.Box(new Rect(4.5f * scr.x, 0.25f * scr.y, 2f * scr.x, 0.25f * scr.y), selectedItem.Name, boxStyle); //Displaying the name of the item  
                    GUI.Box(new Rect(4.5f * scr.x, 2.5f * scr.y, 2f * scr.x, 1f * scr.y), selectedItem.Description, descStyle);//Displaying the Description of the item
                    GUI.skin = invSkin;
                    GUI.Box(new Rect(4.5f * scr.x, 0.5f * scr.y, 2f * scr.x, 2f * scr.y), selectedItem.Icon);        //Displaying the Picture of the item
                    GUI.Box(new Rect(4.5f * scr.x, 2.5f * scr.y, 2f * scr.x, 1f * scr.y), selectedItem.Description, descStyle);//Displaying the Description of the item
                    GUI.Box(new Rect(6.5f * scr.x, 0.5f * scr.y, 1f * scr.x, 0.25f * scr.y), "Amount :" + selectedItem.Amount);//Displaying Amount
                    GUI.Box(new Rect(6.5f * scr.x, 0.75f * scr.y, 1f * scr.x, 0.25f * scr.y), "Heal :" + selectedItem.Heal);
                    GUI.Box(new Rect(6.5f * scr.x, 1f * scr.y, 1f * scr.x, 0.25f * scr.y), "Armour :" + selectedItem.Armour);
                    GUI.Box(new Rect(6.5f * scr.x, 1.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Damage :" + selectedItem.Damage);
                    GUI.Box(new Rect(6.5f * scr.x, 1.5f * scr.y, 1f * scr.x, 0.25f * scr.y), "Value :" + selectedItem.Value);
                    GUI.skin = null;
                    ItemUse();
                }
                else
                {
                    return;
                }
            }
        }
        private void Display()
        {
            if (!(sortType2 ==ItemType.All ))
            {
                //*WE DONT NEED TO PARSE IT BECAUSE WE ALREADY DONE IT ON TOP
                //ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), sortType); 
                int a = 0;//Amount of that type
                int s = 0;//Slot postion
                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == sortType2) //Find our type
                    {
                        a++; //Increase for each item it finds
                    }
                }
                if (a <= 34)
                {
                    for (int i = 0; i < inv.Count; i++)// For looping to display each item within the inventory
                    {
                        if (inv[i].Type == sortType2)
                        {
                            if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + s * (0.25f * scr.y), 3f * scr.x, 0.25f * scr.y), inv[i].Name,descStyle))
                            {
                                selectedItem = inv[i];
                            }
                            s++;
                        }
                    }

                }
                else //More than 34 items
                {
                    scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, 8.5f * scr.y + ((a - 34) * (0.25f * scr.y))),
                   //Can we see our horizontal scroll bar?//Can we see our vertical scroll bar?
                   false, true);
                    for (int i = 0; i < inv.Count; i++)// For looping to display each item within the inventory
                    {
                        if (inv[i].Type == sortType2)
                        {
                            if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + s * (0.25f * scr.y), 3f * scr.x, 0.25f * scr.y), inv[i].Name,descStyle))
                            {

                                selectedItem = inv[i];
                            }
                            s++;
                        }
                    }
                    GUI.EndScrollView();
                }
            }
            else
            {
                if (inv.Count <= 34)//If we have 34 or less (space at top and bottom)
                {
                    for (int i = 0; i < inv.Count; i++)// For looping to display each item within the inventory
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name, descStyle))
                        {
                            selectedItem = inv[i]; // Selects item if clicked
                        }
                    }
                }
                else // More than 34 items
                {
                    //Our moveable scroll position//Start of our viewable area//Our View window        //Current scrollpos //Scroll Zone (extra space)
                    scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, 8.5f * scr.y + ((inv.Count - 34) * (0.25f * scr.y))),
                        //Can we see our horizontal scroll bar?//Can we see our vertical scroll bar?
                        false, true);
                    for (int i = 0; i < inv.Count; i++)
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, 0 * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name, descStyle))
                        {
                            selectedItem = inv[i];
                        }
                    }
                    //Need to End the scroll view(otherwise cause an error)
                    GUI.EndScrollView();
                }
            }
            

        }
        void ItemUse()
        {
            switch (selectedItem.Type)
            {
                case ItemType.Ingredient:
                    break;
                case ItemType.Potion:
                    break;
                case ItemType.Scroll:
                    break;
                case ItemType.Food:
                    break;
                case ItemType.Armour:
                    if (equipmentSlots[0].curItem == null || selectedItem.Name != equipmentSlots[0].curItem.name)
                    {
                        if (GUI.Button(new Rect(6.5f * scr.x, 2.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Equip", boxStyle))
                        {
                            if (equipmentSlots[0].curItem != null)
                            {
                                Destroy(equipmentSlots[0].curItem);
                            }
                            GameObject curItem = Instantiate(selectedItem.ItemMesh, equipmentSlots[0].location);
                            equipmentSlots[0].curItem = curItem;
                            curItem.name = selectedItem.Name;
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(6.5f * scr.x, 2.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Unequip", boxStyle))
                        {
                            Destroy(equipmentSlots[0].curItem);
                        }
                    }
                    break;
                case ItemType.Weapon:
                    if (equipmentSlots[1].curItem == null || selectedItem.Name != equipmentSlots[1].curItem.name)
                    {
                        if (GUI.Button(new Rect(6.5f * scr.x, 2.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Equip", boxStyle))
                        {
                            if (equipmentSlots[1].curItem != null)
                            {
                                Destroy(equipmentSlots[1].curItem);
                            }
                            GameObject curItem = Instantiate(selectedItem.ItemMesh, equipmentSlots[1].location);
                            equipmentSlots[1].curItem = curItem;
                            curItem.name = selectedItem.Name;
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(6.5f * scr.x, 2.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Unequip", boxStyle))
                        {
                            Destroy(equipmentSlots[1].curItem);
                        }
                    }

                    break;
                case ItemType.Craftable:
                    break;
                case ItemType.Money:
                    break;
                case ItemType.Quest:
                    break;
                case ItemType.Misc:
                    break;
            }
            if (GUI.Button(new Rect(6.5f * scr.x, 3.25f * scr.y, 1f * scr.x, 0.25f * scr.y), "Discard", boxStyle))
            {

                for (int i = 0; i < equipmentSlots.Length; i++)
                {//Check equipped items
                    if (equipmentSlots[i].curItem != null && selectedItem.ItemMesh.name == equipmentSlots[i].curItem.name)
                    {
                        //if yeah delete
                        Destroy(equipmentSlots[i].curItem);
                    }
                }
                //Spawn in front
                GameObject droppedItem = Instantiate(selectedItem.ItemMesh, dropLocation.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
                droppedItem.name = selectedItem.Name;
                droppedItem.AddComponent<Rigidbody>().useGravity = true;
                //reduce or delete
                if (selectedItem.Amount > 1)
                {
                    //Reduce the amount of the item
                    selectedItem.Amount--;
                }
                else
                {
                    //if the item is 0 then we remove the item from the list
                    inv.Remove(selectedItem);
                    selectedItem = null;
                }
                return;
            }
        }
    }

}


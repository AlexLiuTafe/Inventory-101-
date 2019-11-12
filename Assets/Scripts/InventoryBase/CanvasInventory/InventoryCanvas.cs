using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCanvas : MonoBehaviour
{
    public List<Item> inv = new List<Item>();
    public Item selectedItem;
    public bool showInv;

    public ItemType sortType = ItemType.All;

    public ScrollRect view;
    public GameObject invButton;
    public RectTransform content;

    private GameObject buttonPrefab;
    private int buttonIndex;

    [Header("Item Description")]
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    public TMP_Text amountInt;
    public TMP_Text healInt;
    public TMP_Text armourInt;
    public TMP_Text damageInt;
    public TMP_Text valueInt;

    private void Start()
    {
        //Adding list of item into inventory list and call the CreateItem Function from ItemData Script
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(2));
        inv.Add(ItemData.CreateItem(302));
        inv.Add(ItemData.CreateItem(401));
        inv.Add(ItemData.CreateItem(402));

        for (int i = 0; i < inv.Count; i++)
        {
            //                      button prefab,location in the hierarchy
            buttonPrefab = Instantiate(invButton, content);
            //  Make the button name spawn in the canvas same as the item name
            buttonPrefab.GetComponentInChildren<Text>().text = inv[i].Name;
            //  Make the button prefab name same as the item name (in the hierarchy)
            buttonPrefab.name = inv[i].Name;
            //Getting the ButtonHandler component
            ButtonHandler b = buttonPrefab.GetComponent<ButtonHandler>();
            //We assign the i variable for buttonhandler script 
            b.i = i;
            //We assign the target variable in ButtonHandler Script into this gameobject which is canvas
            b.target = this.gameObject;
        }

    }
    private void Update()
    {
        //                      button width, height = but we need to expand the content height base on the button height
        content.sizeDelta = new Vector2(0, 25 * inv.Count);
        if (Input.GetKeyDown(KeyCode.I))
        {
            buttonPrefab = Instantiate(invButton, content);
            inv.Add(ItemData.CreateItem(Random.Range(0, 3)));
            for (int i = 0; i < inv.Count; i++)
            {
                buttonPrefab.GetComponentInChildren<Text>().text = inv[i].Name;
                buttonPrefab.name = inv[i].Name;
                ButtonHandler b = buttonPrefab.GetComponent<ButtonHandler>();
                b.i = i;
                b.target = this.gameObject;
            }
        }
    }
    //public void OnClickButton(int itemID)
    //{
    //    if (selectedItem != null)
    //    {
    //        for (int i = 0; i < inv.Count; i++)
    //        {
    //            //Add on click event VIA script
    //            buttonPrefab.GetComponent<Button>().onClick.AddListener(() =>
    //            {
    //                SelectedItem(itemID);
    //            });
    //        }
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}
    public void SelectedItem(int itemID) // We give the function parameter to know which index we are referring to
    {
        Debug.Log("" + itemID);

        selectedItem = inv[itemID];
        itemImage.sprite = selectedItem.IconCanvas; // Image not working!!
        itemName.text = selectedItem.Name.ToString();
        itemDesc.text = selectedItem.Description.ToString();
        armourInt.text = selectedItem.Armour.ToString();
        healInt.text = selectedItem.Heal.ToString();
        damageInt.text = selectedItem.Damage.ToString();
        amountInt.text = selectedItem.Amount.ToString();
        valueInt.text = selectedItem.Value.ToString();

    }
    public void SortAll()
    {

    }
    public void SortIngredient()
    {
        sortType = ItemType.Ingredient;
        for (int i = 0; i < inv.Count; i++)
        {
            inv[i].Type = sortType;
            selectedItem = inv[i];
            
        }
        
    }
}


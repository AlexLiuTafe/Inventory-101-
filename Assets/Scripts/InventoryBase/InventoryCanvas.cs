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


	private GameObject[] buttonPrefabs;
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
		inv.Add(ItemData.CreateItem(0));
		inv.Add(ItemData.CreateItem(1));
		inv.Add(ItemData.CreateItem(2));
		inv.Add(ItemData.CreateItem(302));
		inv.Add(ItemData.CreateItem(401));
		inv.Add(ItemData.CreateItem(402));

		for (int i = 0; i < inv.Count; i++)
		{

			buttonPrefab = Instantiate(invButton, content);
			buttonPrefab.GetComponentInChildren<Text>().text = inv[i].Name;
			buttonPrefab.name = inv[i].Name;
			buttonPrefab.GetComponent<Button>().onClick.AddListener(() =>
			{
				SelectedItem();
			});
			//for (int x = 0; x < buttonPrefabs.Length; x++)
			//{
			//	buttonIndex = selectedItem.ID;
			//	buttonPrefabs[i] = buttonPrefabs[buttonIndex];
				
			//}
			

		}

	}
	private void Update()
	{

		//OnClickButton();

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
			}

		}
	}
	public void OnClickButton()
	{
		if (selectedItem != null)
		{
			for (int i = 0; i < inv.Count; i++)
			{
				//Add on click event VIA script
				buttonPrefab.GetComponent<Button>().onClick.AddListener(() =>
				{
					SelectedItem();
				});
			}
		}
		else
		{
			return;
		}

	}
	public void SelectedItem()
	{


		for (int i = 0; i < inv.Count; i++)
		{
			selectedItem = inv[i];
			itemImage.sprite = selectedItem.IconCanvas; // Image not working!!
			itemName.text = selectedItem.Name.ToString();
			itemDesc.text = selectedItem.Description.ToString();
			armourInt.text = selectedItem.Armour.ToString();
			healInt.text = selectedItem.Heal.ToString();
			damageInt.text = selectedItem.Damage.ToString();
			amountInt.text = selectedItem.Amount.ToString();
			valueInt.text = selectedItem.Value.ToString();
		}




	}
}


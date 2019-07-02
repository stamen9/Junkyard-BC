using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour {

	public GameObject inventoryPanel;
	public GameObject slotPanel;
	public GameObject inventorySlots;
	public GameObject inventoryItem;
	public ItemDataBase itemDB;
	/*public GameObject Head;
	public GameObject MainHand;
	public GameObject OffHand;
	public GameObject Chest;
	public GameObject Pants;*/

	public int SlotAmount;
	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();

	// Use this for initialization
	void Start () {

		//itemDB = GetComponent<ItemDataBase>();

		//inventoryPanel = GameObject.Find("Inventory Panel");
		//slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;
		for(int i = 0; i<SlotAmount; i++)
		{
			items.Add(new Item());
			slots.Add(Instantiate(inventorySlots));
			slots[i].GetComponent<Slot>().id = i;
			slots[i].transform.SetParent(slotPanel.transform);
			slots[i].transform.position = slotPanel.transform.position;
			slots[i].transform.rotation = slotPanel.transform.rotation;
			slots[i].transform.localScale = slotPanel.transform.localScale;

		}
		/*items.Add(new Item());
		slots.Add(Head);
		slots[48].GetComponent<Slot>().id = SlotAmount;
		items.Add(new Item());
		slots.Add(MainHand);
		slots[49].GetComponent<Slot>().id = SlotAmount+1;
		items.Add(new Item());
		slots.Add(OffHand);
		slots[50].GetComponent<Slot>().id = SlotAmount+2;
		items.Add(new Item());
		slots.Add(Chest);
		slots[51].GetComponent<Slot>().id = SlotAmount+3;
		items.Add(new Item());
		slots.Add(Pants);
		slots[52].GetComponent<Slot>().id = SlotAmount+4;*/
		AddItem(0);
		AddItem(0);
		AddItem(0);
		AddItem(0);
		AddItem(0);

		//inventoryPanel.SetActive(false);
	}

	public void AddItem(int id)
	{

		Item itemToAdd = itemDB.FetchByID(id);
		if (itemToAdd.stackable && CheckIfinInv(itemToAdd))
		{
			for (int i = 0; i < items.Count; i++)
			{
				SlotData data = slots[i].transform.GetChild(0).GetComponent<SlotData>();
				data.amount++;
				//Debug.Log (data.transform);
				data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
				break;
			}
		}
		else
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].ID == -1)
				{
					items[i] = itemToAdd;
					GameObject itemObj = Instantiate(inventoryItem);
					itemObj.GetComponent<SlotData>().item = itemToAdd;
					itemObj.GetComponent<SlotData>().sloti = i;
					itemObj.GetComponent<SlotData>().amount = 1;
					itemObj.transform.SetParent(slots[i].transform);
					itemObj.transform.position = slots[i].transform.position;
					itemObj.transform.rotation = slots[i].transform.rotation;
					itemObj.transform.localScale = slots[i].transform.localScale;

					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					break;
				}
			}
		}
	}

	bool CheckIfinInv(Item it)
	{
		for(int i =0; i< items.Count; i++)
		{
			if (items[i].ID == it.ID)
				return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	}
}

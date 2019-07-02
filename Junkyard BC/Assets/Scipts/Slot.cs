using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IDropHandler{

	public Backpack back;
	public int id;

	public void OnDrop(PointerEventData eventData)
	{
		SlotData dropedItem = eventData.pointerDrag.GetComponent<SlotData>();
		Debug.Log("1--");
		Debug.Log(id);
		Debug.Log(back.items[id]);
		Debug.Log("2--");
		if (back.items[id].ID == -1)
		{
			back.items[dropedItem.sloti] = new Item();
			back.items[id] = dropedItem.item;
			dropedItem.sloti = id;
		}else if(dropedItem.sloti != id)
		{
			//Clean this up !!!!!!!!!!!
			Transform item = this.transform.GetChild(0);
			item.GetComponent<SlotData>().sloti = dropedItem.sloti;
			item.transform.SetParent(back.slots[dropedItem.sloti].transform);
			//make it in item data with setter
			item.transform.position = back.slots[dropedItem.sloti].transform.position;

			dropedItem.sloti = id;
			dropedItem.transform.SetParent(this.transform);
			dropedItem.transform.position = this.transform.position;

			back.items[dropedItem.sloti] = item.GetComponent<SlotData>().item;
			back.items[id] = dropedItem.item;
		}
	}

	// Use this for initialization
	void Start () {
		back = GameObject.Find("InventoryController").GetComponent<Backpack>();
	}

	// Update is called once per frame
	void Update () {

	}
}

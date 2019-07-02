using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotData : MonoBehaviour , IBeginDragHandler , IDragHandler , IEndDragHandler , IPointerEnterHandler ,IPointerExitHandler{

	public Item item;
	public int amount;
	//public GameObject cam;
	public GameObject Pnel;
	public GameObject Slot;
	//public GameObject ItemSlot;
	public int sloti;
	private Vector2 offset;
	private ItemTooltip Tooltip;
	private Backpack inv;



	// Use this for initialization
	void Start () {
		inv = GameObject.Find("InventoryController").GetComponent<Backpack>();
		Tooltip = inv.GetComponent<ItemTooltip>();
		//cam = GameObject.FindGameObjectWithTag("MainCamera");
		Pnel = GameObject.Find("Inventory_Panel");
		Slot = GameObject.Find("Slot_Panel");
		//ItemSlot = this.transform.parent.gameObject;

		offset = new Vector2(Pnel.GetComponent<RectTransform>().anchoredPosition.x + 2 + this.GetComponent<RectTransform>().anchoredPosition.x, Pnel.GetComponent<RectTransform>().anchoredPosition.y + 2 + Slot.GetComponent<RectTransform>().anchoredPosition.y + this.GetComponent<RectTransform>().anchoredPosition.y);

	}

	// Update is called once per frame
	void Update () {
		//offset = new Vector2(Pnel.GetComponent<RectTransform>().anchoredPosition.x + 2 + this.GetComponent<RectTransform>().anchoredPosition.x, Pnel.GetComponent<RectTransform>().anchoredPosition.y + 2 + Slot.GetComponent<RectTransform>().anchoredPosition.y + this.GetComponent<RectTransform>().anchoredPosition.y);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(item != null)
		{

			//offset = new Vector2(Pnel.GetComponent<RectTransform>().anchoredPosition.x + 2 + this.GetComponent<RectTransform>().anchoredPosition.x, Pnel.GetComponent<RectTransform>().anchoredPosition.y + 2 + Slot.GetComponent<RectTransform>().anchoredPosition.y + this.GetComponent<RectTransform>().anchoredPosition.y);
			offset = new Vector2(Screen.width / 2, Screen.height / 2);
			this.transform.SetParent(this.transform.parent.parent.parent.parent);
			this.transform.localPosition = eventData.position- offset;
			this.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}

	}

	public void OnDrag(PointerEventData eventData)
	{
		if (item != null)
		{
			this.transform.localPosition = eventData.position - offset;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		//Debug.Log("end");
		this.transform.SetParent(inv.slots[sloti].transform);
		this.transform.position = inv.slots[sloti].transform.position;
		this.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Tooltip.Activate(item);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Tooltip.Deactivate();   
	}
}

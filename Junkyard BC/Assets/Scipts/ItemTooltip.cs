using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour {

	private Item item;
	private string data;
	public GameObject Tooltip;

	public void Activate(Item item)
	{
		this.item = item;
		ConsDataString();
		Tooltip.SetActive(true); 
	}
	public void Deactivate()
	{
		Tooltip.SetActive(false);
	}

	public void ConsDataString()
	{
		data = "<color=#0473f0><b>" + item.Name + "</b></color>\n\n" + item.ID  + "\n" + "Its an Apple of Gold \n";
		Tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
	}

	// Use this for initialization
	void Start () {
		//Tooltip = GameObject.Find("Tooltip");
		//Tooltip.SetActive(false);

	}

	void Update()
	{
		if(Tooltip.activeSelf)
		{

			Tooltip.transform.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
			//Debug.Log(Input.mousePosition);
			//Debug.Log(Tooltip.transform.position);
		}
	}

}

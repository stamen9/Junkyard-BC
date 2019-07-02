using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item {

	public int ID ;
	public string Name;
	public bool stackable;
	public int Value;
	public string slug;
	public Sprite Sprite;
	public string type; // "Equipment","Usable","Quest","Misc" make it enum eventually

	public Item(int id, string name, bool stack, int val,string sl,string tp)
	{
		ID = id;
		Name = name;
		Value = val;
		stackable = stack;
		slug = sl;

		//Debug.Log (Sprite);
		//Sprite = Resources.Load<Sprite>(Application.streamingAssetsPath +  "/super_pixel_objects_and_items/PNG/" + slug);

		type = tp;
	}
	public void LoadSprite()
	{
		if (Sprite) {
			return;
		}
		//CHECK IF FILE EXIST!!!!
		//Debug.Log(Application.streamingAssetsPath +  "/super_pixel_objects_and_items/PNG/" + slug + ".png");
		Sprite = Resources.Load<Sprite>("ItemIcons/PNG/" + slug );
	}
	public Item()
	{
		ID = -1;
	}
	public virtual void Print()
	{
		Debug.Log(ID);
		Debug.Log(Name);
		Debug.Log(stackable);
		Debug.Log(Value);
		Debug.Log("Items/" + slug);
		Debug.Log(type);
	}
}

[Serializable]
public class ItemCollection 
{
	public Item[] itmeCollection;
}

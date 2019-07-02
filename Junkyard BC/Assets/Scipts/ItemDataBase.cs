using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemDataBase : MonoBehaviour {

	//string[] lines = File.ReadAllLines("../Junkyard BC/Assets/ItemsDatabase.txt");
	public ItemCollection db= new ItemCollection();
	private bool dataHasLoaded = false;
	private string itemDataFileName = "items_data.json";

	// Use this for initialization
	void Start()
	{
		db.itmeCollection = new Item[100];
		int counter = 0;

		for (int i = 0; i< 3; i++)
		{
			/*if(lines[counter + 5]== "Equipment")
			{ 
				db.Add(new Equipment(int.Parse(lines[counter++]),
					lines[counter++], 
					bool.Parse(lines[counter++]), 
					int.Parse(lines[counter++]),
					lines[counter++],
					lines[counter++],
					lines[counter++],
					int.Parse(lines[counter++]),
					int.Parse(lines[counter++]),
					int.Parse(lines[counter++]),
					int.Parse(lines[counter++]),
					int.Parse(lines[counter++]),
					int.Parse(lines[counter++])));
			}
			else*/
			{
				/*db.itmeCollection[i] = (new Item(int.Parse(lines[counter++]),
					lines[counter++],
					bool.Parse(lines[counter++]),
					int.Parse(lines[counter++]),
					lines[counter++],
					lines[counter++]));*/
			}

		}
		//string temp = JsonUtility.ToJson (db.itmeCollection[0]);
		//db[0].Print ();
		//Debug.Log (temp);
		if (!dataHasLoaded) {
			LoadItemData ();
			dataHasLoaded = true;
		}
	}
	private void LoadItemData()
	{
		string filePath = Path.Combine (Application.streamingAssetsPath, itemDataFileName);
		//Debug.Log (Application.streamingAssetsPath);
		if (File.Exists ("../Junkyard BC/Assets/ItemsDatabase2.json")) {
			//Debug.Log ("asd");
			string rawItemDate = File.ReadAllText ("../Junkyard BC/Assets/ItemsDatabase2.json");
			//Debug.Log (rawItemDate);
			ItemCollection parsedItemData = JsonUtility.FromJson<ItemCollection>(rawItemDate);

			db = parsedItemData;
		}
		//db.itmeCollection [0].Print ();
	}
	public Item FetchByID(int id)
	{
		if (!dataHasLoaded) {
			LoadItemData ();
			dataHasLoaded = true;
		}
		for(int i = 0; i < db.itmeCollection.Length; i++)
		{
			if(db.itmeCollection[i].ID == id)
			{
				//Debug.Log (db.itmeCollection [i]);
				db.itmeCollection [i].LoadSprite ();
				return db.itmeCollection[i];
			}
		}
		//Debug.Log (db.itmeCollection);
		return null;
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0; i< 3;i++)
		{
			//db[i].Print();
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Base : MonoBehaviour {

	[Serializable]
	public class BaseResources{
		public int[] ore;
		public int[] wood;
		public int[] arcane;

		public int[] plants;
		public int[] meat;
		public int[] herbs;
	}

	public Charecter[] allHeroes;

	public BaseResources baseResources;

	public string pathToBaseResources;
	public string pathToHeroes;

	//For the sake of GenerateMissions will have a
	//refference to the world map here.
	//Might be redundant/useless/not optimal.
	public WorldMap worldMap;

	//should it be in WorldMap.cs?
	//could be either imo
	public List<Mission> missionList;

	private void LoadResources ()
	{
		string fullPath = Path.Combine (pathToBaseResources, "BaseResources.json");
		if (File.Exists (fullPath)) {
			string rawResourceData = File.ReadAllText (fullPath);
			baseResources = JsonUtility.FromJson<BaseResources> (rawResourceData);
		}
	}
	private void LoadHeroes ()
	{
		Debug.Log (pathToHeroes);
		//if (File.Exists (pathToHeroes)) {
			//Debug.Log (pathToHeroes);
			//DirectoryInfo dir = new DirectoryInfo (pathToHeroes);
			//FileInfo[] info = dir.GetFiles ();
			foreach (string file in Directory.GetFiles(pathToHeroes,"*.json")) {
				Debug.Log (file);
			}
		//}
	}

	//Based on what should the mission be generated?
	//current Zone? If so need to have comunication
	//betwean Base.cs and WorldMap.cs
	public void GenerateAllMissions()
	{
		//The more resources a zone has the harder it is?
		Debug.Log (worldMap);
		int arbitraryDifficulty = worldMap.currentZone.arcaneResources.Length +
		                          worldMap.currentZone.forestResources.Length +
		                          worldMap.currentZone.mineResources.Length +
		                          worldMap.currentZone.plants.Length +
		                          worldMap.currentZone.beasts.Length +
		                          worldMap.currentZone.herbs.Length;



		double easy = arbitraryDifficulty * 0.7f;
		double medium = arbitraryDifficulty;
		double hard = arbitraryDifficulty * 1.3f;
		double IRONMAN = arbitraryDifficulty * 1.8f;
		

		missionList.Add (GenerateSingleMission (easy));
		missionList.Add (GenerateSingleMission (medium));
		/*missionList.Add (GenerateSingleMission (hard));
		missionList.Add (GenerateSingleMission (IRONMAN));*/
	}


	//this whole method is a mess 
	public Mission GenerateSingleMission(double difficulty)
	{
		Mission temp = new Mission();

		//need to think of map generation logic
		//simple idea: iterate over the edges and have a % chance to spawn a tree
		//if a tree spaw
		//ok scrap that new idea: chose 2 point on the map for each
		//make a 4-5 tiles path of trees and keep them in an list
		//for each element in the list roll a % to spawn a tree 
		// iterate and lower the % with every iteration until % < some value

		List<Pair> pairs = new List<Pair>();

		int startX = UnityEngine.Random.Range (10, 53);
		int startY = UnityEngine.Random.Range (10, 53);
		temp.tileInfo [startX, startY] = 0;

		pairs.Add (new Pair (startX, startY));

		int tempX = startX;
		int tempY = startY;
		for (int i = 0; i < 5; i++) {
			//surly this is not the best way to gebere these random numbers
			//maybe i should not write code in 2:49 AM?
			int newX = tempX + UnityEngine.Random.Range (-1, 1);
			int newY = tempY + UnityEngine.Random.Range (-1, 1);
			//WTF was i thinking
			/*while (temp.tileInfo [newX, newY] == 0) {
				newX = tempX + UnityEngine.Random.Range (-1, 1);
				newY = tempY + UnityEngine.Random.Range (-1, 1);
			}*/
			temp.tileInfo [newX, newY] = 0;
			pairs.Add (new Pair (newX, newY));
			tempX = newX;
			tempY = newY;
		}
		int percrntile = 70;
		while (percrntile > 20){
			int len = pairs.Count;
			for (int i = 0 ; i < len ; i++) {
				if (UnityEngine.Random.Range (0, 100) < percrntile--) {
					int newX = tempX + UnityEngine.Random.Range (-1, 1);
					int newY = tempY + UnityEngine.Random.Range (-1, 1);
					temp.tileInfo [newX, newY] = 0;
					pairs.Add (new Pair (newX, newY));
				}
			}
		}

		return temp;
	}

	// Use this for initia	lization
	void Start () {
		LoadHeroes ();
		GenerateAllMissions ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

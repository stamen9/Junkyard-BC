using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class Pair
{
	public int v1;
	public int v2;
	public Pair(int _v1,int _v2)
	{
		v1 = _v1;
		v2 = _v2;
	}
}

[Serializable]
public class Graph{
	public Pair[] graph;

	public List<int> GetConnections(int id)
	{
		List<int> temConnections = new List<int> ();
		//feels like there are a few unnecesery checks happening under the hood :S
		foreach (Pair node in graph) {
			if (node.v1 == id) {
				if (temConnections.Contains (node.v2))
					continue;
				temConnections.Add (node.v2);
			} else if (node.v2 == id) {
				if (temConnections.Contains (node.v1))
					continue;
				temConnections.Add (node.v1);
			}
		}
		return temConnections;
	}
}

public class WorldMap : MonoBehaviour {


	public List<Zone> worldMap;
	public Zone currentZone;

	public string pathToFolder;
	public List<string> zoneFileNames;
	public Graph zoneConnections;

	//change to private?
	public void LoadZoneGraph()
	{
		string fullPath = Path.Combine (pathToFolder, "ZoneGraph.json");
		if (File.Exists (fullPath)) {
			string rawFileDate = File.ReadAllText (fullPath);
			zoneConnections = JsonUtility.FromJson<Graph> (rawFileDate);
		}
	}
	public void LoadAllZones()
	{
		foreach (string fileName in zoneFileNames) {
			string fullPath = Path.Combine (pathToFolder, fileName);
			if (File.Exists (fullPath)) {
				string rawFileDate = File.ReadAllText (fullPath);
				Zone temp = JsonUtility.FromJson<Zone>(rawFileDate);
				worldMap.Add (temp);
			}
		}
	}
	// Use this for initialization

	public void MoveToIndex(int index)
	{
		currentZone = worldMap [index];
		Debug.Log (index);
	}

	// skill : mining = 0 , woodgathering = 1 , arcaneextraction = 2 , foresting = 3 , hunting = 4 , herbalisim = 6
	public void GatherFromCurrentZone(Charecter[] gatherers, int skill)
	{
		int totalSkill = 0;
		for (int i = 0; i < gatherers.Length; i++) {
			totalSkill += gatherers[i].skillBlock.gatheringSkill[skill];
		}
		int[] resourcesGained = currentZone.Gather (totalSkill, skill);
	}

	void Start () {
		LoadZoneGraph ();
		LoadAllZones ();

		currentZone = worldMap [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

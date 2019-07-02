using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIHelper : MonoBehaviour {

	public WorldMap worldMap;
	public GameObject travelPanel;
	public GameObject travelPanelPrefab;

	public GameObject inventoyPanel;
	public GameObject gatherPanel;
	public GameObject missionPanel;

	public Base playerBase;

	public void CloseInventory()
	{
		inventoyPanel.SetActive(false);

	}

	public void OpenInventory()
	{
		inventoyPanel.SetActive(true);
	}

	public void CloseGather()
	{
		gatherPanel.SetActive(false);

	}

	public void OpenGather()
	{
		gatherPanel.SetActive(true);
	}

	public void CloseMissions ()
	{
		missionPanel.SetActive (false);
	}

	public void OpenMissions()
	{
		missionPanel.SetActive(true);
	}

	public void CloseMap()
	{
		travelPanel.SetActive(false);
		foreach (Transform child in travelPanel.transform.GetChild(0)) {
			GameObject.Destroy (child.gameObject);
		}
	}

	public void OpenMap()
	{
	 	List<int> temp = worldMap.zoneConnections.GetConnections (worldMap.currentZone.WorldId);
		travelPanel.SetActive(true);
		foreach (int worldId in temp) {
			Debug.Log (worldId);
			GameObject insance = Instantiate (travelPanelPrefab);
			for (int i = 0; i < worldMap.worldMap.Count; i++) {
				if (worldMap.worldMap [i].WorldId == worldId) {
					
					insance.transform.GetChild (0).GetComponent<Text> ().text = "Mines:" + worldMap.worldMap [i].mineResources.Length + "\n" +
						"Forest: " + worldMap.worldMap [i].forestResources.Length + "\n" +
						"Hunt: " + worldMap.worldMap [i].beasts.Length + "\n" +
						"Herbs: " + worldMap.worldMap [i].herbs.Length + "\n" +
						"Plants: " + worldMap.worldMap [i].plants.Length + "\n" +
						"Arcane: " + worldMap.worldMap [i].arcaneResources.Length + "\n";
					insance.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (() => worldMap.MoveToIndex (i));
					insance.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (() => CloseMap());

					break;
				}
			}
			insance.transform.SetParent (travelPanel.transform.GetChild(0));
			insance.transform.localScale = new Vector3 (1f , 1f,1f);
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dont think ill need the mono behavior
public class Mission : MonoBehaviour {


	public enum MissionType
	{
		
	}

	//why not just hve it as string?
	public MissionType missionType;

	//Either enumerate internaly all the tiles
	//or add a tile list here as well
	public int[,] tileInfo = new int[64, 64];

	//numerate all with integers from 0 to unitsToSpawn.count(+1?)
	public int[,] unitsOnMap = new int[64, 64];

	//maybe should be Unit instaid of charecter?
	//depends on how i decide to make the OOP classes
	public List<Charecter> unitsToSpawn = new List<Charecter>();

	public int baseExpReward;

	//no item/stuff reward for now
	//can't think of a way to systemize the rewards
	//maybe a few list of stuff? 
	//i.e list of items list of resources etc.
	//Can you have an void* array in C#?

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

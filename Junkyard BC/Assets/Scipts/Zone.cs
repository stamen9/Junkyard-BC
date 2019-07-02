using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Zone {

	public enum Resource	{CopperMine, SilverMine, IronMine, GoldMine, MithrilMine,
							 WitheredForest, OakForest, PineForest, EnchantedForest,
					 		 Meat, Herbs, Plants, 
					 		 PureMana, ElementalEssenc, ChaosEssence};
					 
	public List<Resource> resources;
	//currently every zone has its own copy of `rates` and its kinda useless
	//move to WorldMap.cs? alternativly make new class for it and link?
	public float[] rates = 	{1f, 0.6f, 0.8f, 0.4f, 0.1f,
							 0.4f, 0.8f, 0.8f, 0.1f,
							 0.6f, 0.2f, 0.8f,
							 0.9f, 0.4f, 0.05f};

	public int[] mineResources;
	public int[] forestResources;
	public int[] arcaneResources;


	//Consistency hello? Where did the `Resources` go?
	public int[] plants;
	public int[] beasts;
	public int[] herbs;


	public List<int[]> availableResources;
	public int WorldId;

	public Zone()
	{
		availableResources = new List<int[]> ();
		availableResources.Add (forestResources);
		availableResources.Add (mineResources);
		availableResources.Add (arcaneResources);
		availableResources.Add (plants);
		availableResources.Add (beasts);
		availableResources.Add (herbs);
	}

	public int[] Gather(int skill, int res)
	{
		int[] resoult = new int[5];
		foreach(int resource in availableResources[res])
		{
			resoult [resource] += (int)(1f * skill * rates [resource]);
		}
		return resoult;
	}
}

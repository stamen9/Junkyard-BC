using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon {


	public int Attack;
	public int Range;

	public enum DamageType {Phisical = 0, Magical =1};

	public DamageType type;

	public StatBlock WeaponStats;

	public Weapon()
	{
		Attack = 1;
		Range = 1;
		type = DamageType.Phisical;
		WeaponStats = new StatBlock ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

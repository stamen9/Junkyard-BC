using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Charecter {

	public int SpellPower = 0;
	public int HealPower = 4;
	public int NovaPower = 1;

	void Heal(Charecter Target)
	{
		Target.CurrentHealth += SpellPower + HealPower;
		if (Target.MaxHP < Target.CurrentHealth) {
			Target.CurrentHealth = Target.MaxHP;
		}
	}
	void Nova()
	{
		GameObject[] list = GameObject.FindGameObjectsWithTag("Monster");
		//List<Charecter> charectersInRange = new List<Charecter> ();
		foreach (GameObject unit in list) {
			Charecter Char = unit.GetComponent<Charecter>();
			if (Mathf.Abs (Char.posX - this.posX) <= 1 && Mathf.Abs (Char.posY - this.posY) <= 1) {
				//charectersInRange.Add (Char);
				if (Char.isPlayerUnit) {
					Char.Recover (NovaPower);
				} else {
					Char.TakeDamege (NovaPower);
				}
			}
		}
		//GameObject.
	}
	public override void SkillOne (Charecter Target)
	{
		Heal (Target);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rogue : Charecter {

	public BasicRogueActives Basic;


	public delegate void AttackWithTargetDelegate(Charecter target);

	public List<AttackWithTargetDelegate> skills; 



	public override void TakeDamege(int IncomingDamage)
	{
		
		if (Dodge ()) {
			return;
		}
		base.TakeDamege (IncomingDamage);
	}
	public void SliceAndDice(Charecter Target)
	{
		int numOfRandomHits = UnityEngine.Random.Range (2, 3);

		for (int i = 0; i < numOfRandomHits; i++) {
			//due to the way the BasicAttack method works this is not optimal :S
			if (BasicAttack (Target)) {
				Debug.Log ("CD inc!!!");
				SkillOneCDLeft = SkillOneCD;
			}
		}
		Debug.Log ("Slice'nDice");
	}
	public override void CallSelecetdAttack (Charecter Target)
	{
		if(SelectedAttack <= skills.Count)
		{
		skills [SelectedAttack] (Target);
		}
	}
	private bool Dodge()
	{
		Debug.Log ("Dodge!!");
		if (UnityEngine.Random.Range (1, 100) > 95) {
			return true;
		}
		return false;
	}
	void Sprint ()
	{
		SkillTwoCDLeft = SkillTwoCD;
		this.leftMove += 2;
		hasAttackedForTurn=true;
	}
	public override void SkillOne(Charecter Target)
	{
		SliceAndDice (Target);
	}
	public override void SkillTwo(Charecter Target)
	{
		Sprint ();
	}
	// Use this for initialization
	void Start () {
		SkillOneCD = 3;
		SkillTwoCD = 4;

		type = 'm';
		leftMove = maxMove;
		CurrentHealth = MaxHP;
		/*
		skills = new List<AttackWithTargetDelegate> ();
		skills.Add (SliceAndDice);*/

		pathfinder.setNotPassable (this.posX, this.posY);

		this.transform.position = new Vector3 (posX * (0.264f / 2f) + posY * (0.264f), 0.08f, posX * (0.264f * 0.75f));
	}

	public override void DoAttacksCheck()
	{
		if (attemptingAttack>0 && !hasAttackedForTurn ) {

			pathfinder.ClearLines ();
			switch (attemptingAttack)
			{
			case 1:
				pathfinder.SetMoveBorder (1, posX, posY, Color.red);
				Charecter BasicAttackTarget = FindTargetForAttack ();
				if (BasicAttackTarget) {
					BasicAttack (BasicAttackTarget);
				}
				break;
			case 2:
				Debug.Log (SkillOneCDLeft);
				if (SkillOneCDLeft > 0)
					break;
				//maybe bad name :S
				pathfinder.SetMoveBorder (1, posX, posY, Color.red);
				Charecter S2target = FindTargetForAttack ();
				if (S2target) {
					SkillOne (S2target);
				}
				break;
			case 3:
				if (SkillTwoCDLeft > 0)
					break;
				SkillTwo (null);
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		DoAttacksCheck ();

		if (HPBar) {
			HPBar.transform.localScale = new Vector3(((float)CurrentHealth / (float)MaxHP) * 6, 1, 0);
		}
	}
}

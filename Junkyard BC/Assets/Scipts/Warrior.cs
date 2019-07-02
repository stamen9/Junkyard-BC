using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Charecter {


	int defenciestance = 0;

	void Bash(Charecter Target)
	{
		Target.TakeDamege (1);
		Target.stunned = true;
	}
	void DefensiveStance()
	{
		defenciestance = 2;
	}

	public override void SkillOne(Charecter Target)
	{
		Bash (Target);
	}
	public override void SkillTwo(Charecter Target)
	{
		DefensiveStance ();
	}
	public override void TakeDamege(int IncomingDamage)
	{

		if (defenciestance > 0) {
			IncomingDamage -= 1;
		}
		base.TakeDamege (IncomingDamage);
	}
	public virtual void EndOfTurnEffects()
	{
		base.EndOfTurnEffects ();
		if (defenciestance > 0) {
			defenciestance -= 1;
		}
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
	
	// Update is called once per frame
	void Update () {

		DoAttacksCheck ();

		if (HPBar) {
			HPBar.transform.localScale = new Vector3(((float)CurrentHealth / (float)MaxHP) * 6, 1, 0);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

[Serializable]
public class Charecter : Unit  {


	//not sure how to best go about conditions hmm....
	struct Conditions
	{
		
	}

	public Pathfinding pathfinder;
	public int maxMove;
	public int leftMove;

	public int attemptingAttack = 0;

	public int MaxHP;
	public int CurrentHealth;

	public bool isPlayerUnit;

	public int SelectedAttack = 0;
	public void SetSelectedAttack(int id)
	{
		SelectedAttack = id;
	}

	public Weapon equipedWeapon;
	public int tempAttack;

	public bool setToMove;
	public bool setToAttack;
	public GameObject HPBar;

	public int initiative;

	public StatBlock BaseCharecterStats;

	public Skills skillBlock;

	protected bool hasAttackedForTurn = false;


	protected int SkillOneCD = 0;
	protected int SkillTwoCD = 0;
	protected int SkillOneCDLeft = 0;
	protected int SkillTwoCDLeft = 0;

	public bool stunned = false;

	public Charecter()
	{
		BaseCharecterStats = new StatBlock ();
		equipedWeapon = new Weapon ();
	}

	/*public int CompareTo(Charecter comp)
	{
		if (comp == null)
			return 0;
		return this.initiative.CompareTo (comp.initiative);
	}*/

	public virtual void RollInitiative() 
	{
		initiative = UnityEngine.Random.Range (1, 20);
		initiative += (BaseCharecterStats.DEX/2);
		//Add kill status.
	}

	public virtual void TakeDamege(int IncomingDamage) 
	{
		CurrentHealth = CurrentHealth - IncomingDamage;
		//Add kill status.
		if (CurrentHealth <= 0) {
			this.gameObject.SetActive (false);
		}
	}

	public virtual void EndOfTurnEffects()
	{
		leftMove = maxMove;
		pathfinder.ClearLines ();
		attemptingAttack = 0;
		setToMove = false;
		setToAttack = false;
		stunned = false;
		hasAttackedForTurn = false;
		SkillOneCDLeft--;
		SkillTwoCDLeft--;
	}
	//Don't think anything uses this constructor?
	public Charecter(int x, int y,int sx, int sy)
	{
		type = 'm';
		posX = x;
		posY = y;
		sizeX = sx;
		sizeY = sy;
		equipedWeapon = new Weapon ();
	}
	// Use this for initialization
	void Start () {
		
		type = 'm';
		leftMove = maxMove;
		CurrentHealth = MaxHP;
		tempAttack = 0;

		pathfinder.setNotPassable (this.posX, this.posY);
	}

	public virtual void DoAttacksCheck()
	{
		if (attemptingAttack>0) {
			switch (attemptingAttack)
			{
			case 1:
				FindTargetForAttack ();
				break;
			case 2:
				//SkillOne ();
				break;
			case 3:
				//SkillTwo ();
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {

		DoAttacksCheck ();

		//HPBar.fillAmount = (float)CurrentHealth / (float)MaxHP;
	}

	public void Recover(int hp)
	{
		this.CurrentHealth += hp;
		if (this.MaxHP < this.CurrentHealth) {
			this.CurrentHealth = this.MaxHP;
		}
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) ) {
			/*if (GameObject.FindGameObjectWithTag ("GameController").GetComponent<Grid> ().players[GameObject.FindGameObjectWithTag ("GameController").GetComponent<Grid> ().currentPlayer] == owner) {
				GameObject.FindGameObjectWithTag ("GameController").GetComponent<Grid> ().currentSelectedUnit = this.gameObject;
				Debug.Log ("Monster::OnMouseOver called?");
			} else {
				Debug.Log ("This is an enamy monster");
			}*/
		}
	}
	public void Destroy()
	{
		Destroy (this.gameObject);
	}
	public Charecter FindTargetForAttack()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				//this is kind of pointless V but im to lazy yo remove it now :S
				if (hit.transform.gameObject) {
					Charecter target = hit.transform.gameObject.GetComponent<Charecter> ();
					return target;
				} else {
					Debug.Log ("No hit");
					return null;
				}

			}
		}
		return null;
	}

	//This is kind a useless step
	public virtual void CallSelecetdAttack(Charecter Target)
	{
		BasicAttack(Target);
	}
	public virtual bool BasicAttack(Charecter Target, bool isAIAttack = false)
	{
		bool success = false;
		if (equipedWeapon.Range == 1) {
			
			Debug.Log (Target);
			if (((Target.posX - posX) >= -1 && (Target.posX - posX) <= 1) && ((Target.posY - posY) >= -1 && (Target.posY - posY) <= 1)) {
				success = true;
			}
		} else {
			if (pathfinder.FindPath (Target) <= equipedWeapon.Range) {
				success = true;
			}
		}
		attemptingAttack = 0;
		pathfinder.ClearLines ();
		if (success) {
			if (Target.isPlayerUnit && !isAIAttack) {
				return false;
			}
			hasAttackedForTurn = true;
			Target.TakeDamege (equipedWeapon.Attack);
		}
		return success;
	}



	public virtual void SkillOne (Charecter Target)
	{
		Debug.Log("Base Skill One");
	}

	public virtual void SkillTwo (Charecter Target)
	{
		Debug.Log("Base Skill One");
	}
}

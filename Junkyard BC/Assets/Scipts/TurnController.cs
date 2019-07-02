using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnController : MonoBehaviour /*NetworkBehaviour*/ {

	//public List<Monster> Player1Monsters;
	//public List<Monster> Player2Monsters;

	bool waitingForAIToFinish = false;

	public List<Charecter> TurnOrder;
	public AIController AI;
	//public SortedList<int ,GameObject> TurnOrder;

	public Grid grid;
	// Use this for initialization
	void Start () {
		GameObject[] foundMonsters = GameObject.FindGameObjectsWithTag ("Monster");
		Debug.Log (foundMonsters);
		foreach (GameObject monster in foundMonsters) {
			Charecter found = monster.GetComponent<Charecter> ();
			found.RollInitiative ();
			TurnOrder.Add(found);
			//TurnOrder2.
		}
		TurnOrder.Sort ((Char1, Char2) =>
			{
				return Char2.initiative.CompareTo(Char1.initiative);
			});

		grid.currentSelectedUnit = TurnOrder[0].transform.gameObject;



		if (!TurnOrder [0].isPlayerUnit) {
			//implement some AI logic here
			AI.BasicAILogic (TurnOrder [0]);
			EndCurrentUnitTurn();
		}
	}

	public void SetAttackOfCurrentUnit(int id)
	{
		TurnOrder [0].SetSelectedAttack (id - 1);
	}
	public void EndCurrentUnitTurn ()
	{
		Charecter temp = TurnOrder [0];
		temp.EndOfTurnEffects ();
		TurnOrder.RemoveAt (0);
		if(temp.gameObject.activeSelf)
			TurnOrder.Add (temp);
		grid.currentSelectedUnit = TurnOrder [0].transform.gameObject;
		if(TurnOrder[0].stunned)
		{
			EndCurrentUnitTurn();
		}
			
		if (!TurnOrder [0].isPlayerUnit) {
			//implement some AI logic here
			if (AI.busy) {
				waitingForAIToFinish = true;
				return;
			}
			AI.BasicAILogic (TurnOrder [0]);
			//Debug.Log("Skiping Enemy Turn");
			EndCurrentUnitTurn();
		}
	}
	/*[Command]
	public void CmdSwitchPlayer()
	{
		Debug.Log (grid.players[grid.currentPlayer]);
		Debug.Log (grid.currentPlayer);
		if (grid.players[grid.currentPlayer].isLocalPlayer) {
			if (grid.currentPlayer == 0) {
				grid.currentPlayer = 1;
			} else {
				grid.currentPlayer = 0;
			}
		} else {
			Debug.Log ("Its not your turn to end!");
		}
	}*/
	// Update is called once per frame
	void Update () {
		if (waitingForAIToFinish) {
			if (!AI.busy) {
				AI.BasicAILogic (TurnOrder [0]);
				//Debug.Log("Skiping Enemy Turn");
				waitingForAIToFinish = false;
				EndCurrentUnitTurn();

			}
		}
	}
}

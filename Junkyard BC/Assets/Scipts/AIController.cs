using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	public Grid map;
	// Use this for initialization
	Charecter enemy;
	Charecter target;

	//Kinda 2 bools that do the same thing?
	public bool busy = false;
	bool needToAttemtAttack ;

	void Start () {
		
	}
	HexNode FindClosestTarget(Charecter closesTo)
	{
		Debug.Log("Start finding Target!");
		GameObject[] found = GameObject.FindGameObjectsWithTag ("Monster");
		List<Charecter> list = new List<Charecter>();
		foreach(GameObject obj in found)
		{
			list.Add (obj.GetComponent<Charecter> ());
		}
		float lowestDist = 999999f;
		Charecter closest = null;
		foreach (Charecter charecter in list) {
			if (!charecter.isPlayerUnit) {
				continue;
			}
			float x = charecter.posX - closesTo.posX;
			float y = charecter.posY - closesTo.posY;
			float rawDist = Mathf.Sqrt(x*x + y*y);
			if (lowestDist > rawDist) {
				lowestDist = rawDist;
				closest = charecter;
			}
		}
		if (closest == null) {
			//Add AI win logic here
			return null;
		}
		Debug.Log ("Found target: " + closest.name);
		target = closest;
		List<HexNode> resoults = map.GetHexNeighbours (map.hexNodeGrid [closest.posX, closest.posY]);
		if (resoults.Count == 0) {
			return null;
		}
		HexNode result =resoults[0];
	
		Debug.Log (result.posX);
		return result;
	}
	void MoveCloseToTarget (Charecter Enemy, HexNode target)
	{
		Enemy.pathfinder.OnCall (target.posX, target.posY , Enemy.transform.gameObject, true);
	}
	void AttackTarget(Charecter Attacker, Charecter Defender)
	{
		Debug.Log ("Attack!");
		Attacker.BasicAttack (Defender,true);
	}
	public void BasicAILogic(Charecter Enemy)
	{
		busy = true;
		enemy = Enemy;
		HexNode targetNeighbour = FindClosestTarget (Enemy);
		if (targetNeighbour == null) {
			needToAttemtAttack = true;
			return;
		}
		MoveCloseToTarget (Enemy, targetNeighbour);
		Debug.Log ("questionable cycle");
		/*while (Enemy.pathfinder.isMoving) {
			Debug.Log("Wait for it......");
			//continue;
		}*/

		needToAttemtAttack = true;
		//AttackTarget (Enemy , target);
	}
	// Update is called once per frame
	void Update () {


		if (needToAttemtAttack) {
			
			if (!target.pathfinder.isMoving) {
				AttackTarget (enemy , target);
				needToAttemtAttack = false;
				busy = false;
			}
		}
	}
}

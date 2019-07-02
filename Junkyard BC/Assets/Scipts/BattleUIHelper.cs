using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BattleUIHelper : MonoBehaviour {
	public TurnController g_turnController;
	public GameObject instanceMonster;
	public Grid grid;
	public GameObject UnitCanvas;
	// Use this for initialization
	void Start () {
		g_turnController = GameObject.Find ("TurnController").GetComponent<TurnController> ();
		grid = GameObject.Find ("Map").GetComponent<Grid> ();
		UnitCanvas = GameObject.Find ("Canvas");

			//GameObject.Find ("Spawn Monster").GetComponent<Button>().onClick.AddListener(SpawnMonster);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RefillMonsterMovement ()
	{
		/*foreach (Monster monsP1 in g_turnController.Player1Monsters) {
			monsP1.leftMove = monsP1.maxMove;
		}

		foreach (Monster monsP2 in g_turnController.Player2Monsters) {
			monsP2.leftMove = monsP2.maxMove;
		}*/
	}


	public void SpawnMonster()
	{

		GameObject clone = (GameObject)Instantiate (instanceMonster, new Vector3 (1 * 0.16f, 0, 1 * 0.16f), Quaternion.Euler (0, 0, 0), UnitCanvas.transform);

		clone.GetComponent<Charecter> ().posX = 1;
		clone.GetComponent<Charecter> ().posY = 1;
		//g_turnController.Player1Monsters.Add (clone.GetComponent<Monster> ());
		//clone.GetComponent<Monster> ().owner = this.transform.GetComponent<Player> ();
		Debug.Log ("CmdSpawnMonster Called");



		//else if (isClient) {
		//NetworkServer.SpawnWithClientAuthority (clone, connectionToClient);
		//NetworkServer.SpawnWithClientAuthority (clone, connectionToServer);
		//}
	
			
	}
		
}

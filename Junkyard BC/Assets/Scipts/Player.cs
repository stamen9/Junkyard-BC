using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Player : MonoBehaviour {

	public int id;

	public Grid mapGrid;
	// Use this for initialization
	void Start () {
		//GameObject.FindGameObjectWithTag ("GameController").GetComponent<Grid> ().players.Add(this);
		/*if (id == 1) {
			mapGrid.playerOne = this;
		} else {
			mapGrid.playerTwo = this;
		}*/
		/*id = int.Parse(netId.ToString());
		Debug.Log ("Player " + id);
		if (isServer) {
			this.transform.name = "host" + id;
		}
		else
		{
			this.transform.name = "client" + id;
		}*/
	}

	/*void OnStartLocalPlayer()
	{
		GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<Grid>().playerOne = this;
		Debug.Log ("Stuff");
	}*/
	// Update is called once per frame
	void Update () {
		
	}
}

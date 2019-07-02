using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	//in order Red , Green , Blue , Black , ColorrLess
	public int[] costs= new int[5];
	public string Name;
	public int cardIndex;
	public char type;
	public Sprite cardImage;
	public GameObject prop;
	public GameObject card;
	public int Attack;
	public int Health;


	// Use this for initialization
	void Start () {
		card = this.gameObject;
		card.transform.GetChild (0).GetComponent<Image> ().sprite = cardImage;
		card.transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = Name;
		card.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = Attack.ToString();
		card.transform.GetChild (4).GetChild (0).GetComponent<Text> ().text = Health.ToString();
		for (int i = 0; i < 5; i++) {
			card.transform.GetChild (5).GetChild (i).GetChild(0).GetComponent<Text> ().text = costs[i].ToString();
			Debug.Log (card.transform.GetChild (5).GetChild (i).GetChild(0));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

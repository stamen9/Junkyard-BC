using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public List<Card> deck = new List<Card>();

    public Card[] TakeCard()
    {
        System.Random r = new System.Random();
        Random.InitState(0);
        int card1 = r.Next(0 , deck.Count);
        int card2 = r.Next(0 , deck.Count);
        int card3 = r.Next(0 , deck.Count);

        Card[] DrawnCards = new Card[3];
        DrawnCards[0] = deck[card1];
        DrawnCards[1] = deck[card2];
        DrawnCards[2] = deck[card3];

        return DrawnCards;
    }


    // Use this for initialization
    void Start() {
        
    }
	
	// Update is called once per frame
	void Update () {
	}
}

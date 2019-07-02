using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreator : MonoBehaviour {

    //limit on deck 60

    public Deck newDeck = new Deck();
    public Collection collection;



    //za dovurshvae
    public void addCard(Card clickCard)
    {
        newDeck.deck.Add(clickCard);

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

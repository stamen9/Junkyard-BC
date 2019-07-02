using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour {


    public List<Card> collection = new List<Card>();
    public GameObject instance;


    public void Visualization()
    {
        for (int i = 0; i < 5; i++)
        {
            Card temp = new Card();
            temp.Name = "SomeGosho" + i; 
            collection.Add(temp);
        }

        foreach (Card card in collection)
        {
            GameObject temp = (GameObject)Instantiate(instance, new Vector3(0,0,0), Quaternion.identity);
            Card temp2 = temp.GetComponent<Card>();
            temp2.Name = card.Name;
        }
    }

	// Use this for initialization
	void Start () {
        Visualization();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

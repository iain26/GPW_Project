using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

    public GameObject cardPrefab;
    public static List<Card> deck = new List<Card>();
    public List<string> trait1 = new List<string>();
    public List<string> trait2 = new List<string>();
    int deckSize = 200;

    // Use this for initialization
    void Start() {

        for (int i = 0; i < deckSize; i++) {
            AddCardToDeck(i);
        }
        foreach (Card card in deck)
        {
            card.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void AddCardToDeck(int count) {
        GameObject card = (GameObject)Instantiate(cardPrefab);
        card.name = "Card " + (count +1).ToString();
        card.transform.SetParent(GameObject.Find("Draw").transform);
        card.transform.position = new Vector3((float)Screen.width / 10f, (float)Screen.height / 1.25f + 200, 0);
        deck.Add(card.GetComponent<Card>());
    }
		
	
	// Update is called once per frame
	void Update () { 
		foreach (Card card in deck) {
			card.transform.localScale = new Vector3(1f, 1f, 1f);
        }
	}
}

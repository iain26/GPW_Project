using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Selection : MonoBehaviour {

	float offset = 0f;
	float handOffset = 0f;
	float offsetIncre = 0f;

	int drawnCards = 1;

	Vector3 deckPos;
	Vector3 tablePos;
	Vector3 handPos;
	Vector3 discardPos;
	Vector3 rejectionPos;

	bool sampled = true;

	public List<Card> cardsToDraw = new List<Card>();
	public List<Card> cardsOnTable = new List<Card>();
	public List<Card> cardsInDiscard = new List<Card>();
	public List<Card> cardsInHand = new List<Card>();
	public List<Card> cardsInRejection = new List<Card>();

    GameObject rejectedAlert;
    GameObject helpAlert;
    GameObject harmAlert;

    [SerializeField]
    float time = 1f;

    //public delegate void SaveToExternal(string actionPerformed);
    //public static event SaveToExternal onAction;

    public delegate void CommunityStats(string stat, float changeAmount);
    public static event CommunityStats onSample;

    // Use this for initialization
    void Start () {
        //sets positions cards can be on screen depending on screen size
        offsetIncre = ((float)Screen.width / 6.8f) * 1.25f;
        InitialisePositions();
		cardsToDraw = Deck.deck;
        DeckToTable();

        rejectedAlert = GameObject.Find("RejectedAlert");
        helpAlert = GameObject.Find("AcceptedHelped");
        harmAlert = GameObject.Find("AcceptedHarmed");

        rejectedAlert.SetActive(false);
        helpAlert.SetActive(false);
        harmAlert.SetActive(false);
    }

    void InitialisePositions()
    {
        deckPos = new Vector3((float)Screen.width / 10f, (float)Screen.height / 1.25f + 200, 0);
        tablePos = new Vector3((float)Screen.width / 10f, (float)Screen.height / 2.25f, 0);
        handPos = new Vector3((float)Screen.width / 10f, (float)Screen.height / 15f - 200, 0);
        discardPos = new Vector3((float)Screen.width / 4f, (float)Screen.height / 1.25f + 200, 0);
        rejectionPos = new Vector3((float)Screen.width / 2f, (float)Screen.height / 1.25f + 200, 0);
    }

	void OnEnable(){
		//Card.onDraw += DeckToTable;
		Card.onSample += TableToHand;
	}

	void OnDisable(){
		//Card.onDraw -= DeckToTable;
		Card.onSample -= TableToHand;
	}

	void DeckToTable(){
//		StartCoroutine (MoveCard(card, handPos));
		//if (sampled)
        {
           // onAction("Drew Cards");
			sampled = false;
			for (int i = 0; i < 5; i++) {
				cardsToDraw [(cardsToDraw.Count - 1)].gameObject.transform.position = new Vector3 (tablePos.x + offset, tablePos.y, 0f);
				cardsToDraw [(cardsToDraw.Count - 1)].drawn = true;
				cardsToDraw [(cardsToDraw.Count - 1)].gameObject.transform.SetParent (GameObject.Find ("Table").transform);
				offset += offsetIncre;
				cardsOnTable.Add (cardsToDraw [(cardsToDraw.Count - 1)]);
				cardsToDraw.Remove (cardsToDraw [(cardsToDraw.Count - 1)]);
				drawnCards++;
			}
			offset = 0f;
		}
	}

	IEnumerator MoveCard(GameObject card, Vector3 target){
		while(card.transform.position != target)
		{
			if(card.transform.position == target){Debug.Log ("target reached");}
			card.transform.position = Vector3.Lerp (card.transform.position, target, 0.1f);
			yield return 0;
		}
		yield return 0;
	}

	void TableToHand(Card cardToHand){
		int rejectRandom = Random.Range (0, 3);
        int helpRandom = Random.Range(1, 9);
        if (rejectRandom > 0)
        {
           // onAction("Selected Card");
            if (cardToHand.helpHarmStat >= helpRandom)
            {
                StartCoroutine(EnableWaitDisable(helpAlert, time));
                cardsInHand.Add(cardToHand.GetComponent<Card>());
                cardToHand.gameObject.transform.position = new Vector3(handPos.x + handOffset, handPos.y, 0f);
                cardToHand.gameObject.transform.SetParent(GameObject.Find("Hand").transform);
                cardToHand.gameObject.GetComponent<Button>().enabled = false;
                handOffset += offsetIncre;
                //cardToHand.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                onSample("Money", Random.Range(10, 25));
                onSample("Population", 1f);
                onSample("Satisfaction", 0.05f);
                cardsOnTable.Remove(cardToHand.GetComponent<Card>());
            }
            else
            {
                StartCoroutine(EnableWaitDisable(harmAlert, time));
                cardsInHand.Add(cardToHand.GetComponent<Card>());
                cardToHand.gameObject.transform.position = new Vector3(handPos.x + handOffset, handPos.y, 0f);
                cardToHand.gameObject.transform.SetParent(GameObject.Find("Hand").transform);
                cardToHand.gameObject.GetComponent<Button>().enabled = false;
                handOffset += offsetIncre;
                // cardToHand.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(220, 0, 255, 255);
                onSample("Money", Random.Range(5, 10));
                onSample("Population", 1f);
                onSample("Satisfaction", -0.05f);
                cardsOnTable.Remove(cardToHand.GetComponent<Card>());
            }
		} else {
            StartCoroutine(EnableWaitDisable(rejectedAlert, time));
            cardsInRejection.Add (cardToHand.GetComponent<Card> ());
			cardToHand.gameObject.transform.position = rejectionPos;
			cardToHand.gameObject.transform.SetParent (GameObject.Find ("Rejection").transform);
			cardToHand.gameObject.GetComponent<Button> ().enabled = false;
           // cardToHand.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            cardsOnTable.Remove (cardToHand.GetComponent<Card> ());
		}
		Discard ();
		sampled = true;
	}

    IEnumerator EnableWaitDisable(GameObject alert, float time)
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(time);
        alert.SetActive(false);
    }

	void Discard (){
//		for(int i = 0; i < 5; i++) 
		foreach(Card cardOnTable in cardsOnTable){
			cardsInDiscard.Add (cardOnTable);
			cardOnTable.gameObject.transform.position = discardPos;
			cardOnTable.gameObject.transform.SetParent (GameObject.Find ("Discard").transform);
			cardOnTable.gameObject.GetComponent<Button>().enabled = false;
			cardOnTable.drawn = false;
		}
		cardsOnTable.Clear ();
        DeckToTable();
    }

	void CheckWin(){
//		int win = 0;
//		foreach (GameObject card in heldCards) {
//			if (card.GetComponent<Card>().name == "Iain") {
//				win++;
//			}
//		}
//		if (win > 4) {
//			StartCoroutine (Wait ());
//		}
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds (2f);
		Application.LoadLevel ("Win");
	}

	void ShuffleDiscardBackToDeck(){
		foreach(Card cardInDiscard in cardsInDiscard) {
			cardInDiscard.transform.position = deckPos;
			cardInDiscard.transform.GetComponent<Button> ().enabled = true;
			cardsToDraw.Add (cardInDiscard);
			cardInDiscard.transform.SetParent (GameObject.Find ("Draw").transform);
		}
		cardsInDiscard.Clear ();
	}

	// Update is called once per frame
	void Update () {
		if(cardsToDraw.Count < 5){
			ShuffleDiscardBackToDeck();
		}
		
		//CheckWin ();

		//checks screen width each frame and 
		offsetIncre = ((float)Screen.width / 6.8f) * 1.25f;
	}
}

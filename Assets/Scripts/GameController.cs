using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    [Header("Game Info")]
    public int handSize = 6;
    public int deckSize = 30;

    [Header("Fields")]
    public GameObject handPanel;
    public GameObject cardPrefab;

    //[HideInInspector]
    public List<Vector2> deck;
    public List<Vector2> hand;
    [HideInInspector]
    public Card card;

    void Start()
    {
        // Initialize the deck and hand so we have something to add to
        deck = new List<Vector2>();
        hand = new List<Vector2>();

        // Build the deck and shuffle
        deck = BuildDeck();
        hand = Deal(deck);


        //foreach (Vector2 vector in hand)
        //{
        //    print(string.Format("Vector is ({0}, {1})", vector.x, vector.y));
        //}

    }


    // TODO fix this, but for now it will be hardcoded ot make all of the cards
    public List<Vector2> BuildDeck()
    {
        //Debug.Log("BuildDeck was called");

        var tempDeck = new List<Vector2>();
        var temp = new Vector2();
        int random;


        for (int i = 1; i <= 17; i++)
        {
            for (int j = 1; j <= 17; j++)
            {
                temp = new Vector2(i, j);
                deck.Add(temp);
            }
        }


        for (int i = 0; i < deckSize; i++)
        {
            random = Random.Range(0, deck.Count);
            tempDeck.Add(deck[random]);
            deck.RemoveAt(random);            
        }
        return tempDeck;
    }

    public Vector2 Draw(List<Vector2> deck)
    {
        var temp = deck[0];
        deck.RemoveAt(0);
        return temp;
    }

    public List<Vector2> Deal(List<Vector2>deck)
    {
        var temp = new List<Vector2>();
        Card current;

        for (int i = 0; i < handSize - 1; i++)
        {
            temp.Add(Draw(deck));
        }

        for (int i = 0; i < temp.Count; i++)
        {
            //  TODO Change this to PhotonNetwork.Instantiate???
            current = GameObject.Instantiate(cardPrefab).GetComponent<Card>();
            current.transform.SetParent(handPanel.transform);
            current.AssignCard(temp[i]);
        }

        return temp;
    }


    // An overload method that can be used to build a deck that has the cards in List<Card> deck
    public void BuildDeck(List<Card> deck)
    {
        return;
    }
}

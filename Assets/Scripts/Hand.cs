using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand
{
    public Card cardPrefab;
    public GameObject handPanel;

    private int handSize;
    private int deckSize;
    private int numCardTypes;


    public List<Vector2> deck;
    public List<Card> hand;

    public Hand(int deckSize, int handSize, int numCardTypes, Card cardPrefab)
    {
        if (hand == null)
        {
            hand = new List<Card>();
        }
        this.deckSize = deckSize;
        this.handSize = handSize;
        this.numCardTypes = numCardTypes;
        this.cardPrefab = cardPrefab;
        handPanel = GameObject.Find("Hand");

        var tempDeck = PlayerPrefsX.GetVector2Array("Deck");
        this.deck = BuildDeck(tempDeck);

        if (deck == null || deck[deckSize - 1] == Vector2.zero)
        {
            Debug.Log("Using random deck");
            this.deck = BuildDeck(deckSize);
        }

        hand = Deal(deck);
    }

    private List<Vector2> BuildDeck(int deckSize)
    {
        var deck = new List<Vector2>();
        var deckValues = new List<Vector2>();
        int random;

        for (int i = 1; i < numCardTypes + 1; i++)
        {
            for (int j = 1; j < numCardTypes + 1; j++)
            {         
                deckValues.Add(new Vector2(i, j));
            }
        }

        for (int i = 0; i < deckSize; i++)
        {
            random = Random.Range(0, deckValues.Count);
            deck.Add(deckValues[random]);
        }

        return deck;
    }

    // Overload method for when you can input your own deck of cards to be shuffled.
    private List<Vector2> BuildDeck(Vector2[] deck)
    {
        var myDeck = new List<Vector2>();

        for (int i = 0; i < deck.Length; i++)
        {
            myDeck.Add(deck[Random.Range(0, deck.Length)]);
        }

        //foreach (Vector2 vector in myDeck)
        //{
        //    Debug.Log(string.Format("x is {0}, y is {1}", vector.x, vector.y));
        //}

        return myDeck;
    }

    public List<Card> Deal(List<Vector2> deck)
    {
        var temp = new List<Card>();

        for (int i = 0; i < handSize; i++)
        {
            temp.Add(Draw(deck));
        }
        return temp;
    }

    public Card Draw(List<Vector2> deck)
    {
        var card = GameObject.Instantiate(cardPrefab);
        card.BuildCard((int)deck[0].x, (int)deck[0].y);
        card.transform.SetParent(handPanel.transform);
        deck.RemoveAt(0);

        return card;
    }
}

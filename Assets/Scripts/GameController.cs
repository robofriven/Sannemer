using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [Header("Health and Score")]
    public int startingHealth = 20;
    [SerializeField] private int round;
    [SerializeField] private int myHealth;
    [SerializeField] private int oppHealth;
    [SerializeField] private int myScore;
    [SerializeField] private int oppScore;


    [Header("Game Info")]
    public int handSize = 6;
    public int deckSize = 30;

    [Header("Fields")]
    public GameObject handPanel;
    public GameObject attackPanel;
    public GameObject defensePanel;
    public GameObject oppAttPanel;
    public GameObject oppDefPanel;
    [Header("Prefabs")]
    public GameObject cardPrefab;

    [HideInInspector]
    public List<Vector2> deck;
    [HideInInspector]
    public List<Vector2> hand;
    [HideInInspector]
    public Card card;

    private int myAtt;
    private int myDef;
    private int oppAtt;
    private int oppDef;

    void Start()
    {
        // Initialize the deck and hand so we have something to add to
        deck = new List<Vector2>();
        hand = new List<Vector2>();

        // Start a new round
        round = 1;
        StartRound();


        //foreach (Vector2 vector in hand)
        //{
        //    print(string.Format("Vector is ({0}, {1})", vector.x, vector.y));
        //}

    }

    // This method will be used to initialize a new round
    // Will update the score, reset the health, reshuffle and redeal
    public void StartRound(int MYSCORE = 0, int OPPSCORE = 0)
    {
        if (round <= 3)
        {
            if (MYSCORE < 0)
                MYSCORE = 0;

            if (OPPSCORE < 0)
                OPPSCORE = 0;

            myScore += MYSCORE;
            oppScore += OPPSCORE;

            myHealth = startingHealth;
            oppHealth = startingHealth;

            // Build the deck and shuffle
            deck = BuildDeck();
            hand = Deal(deck);
        }
        else
        {
            Debug.Log("Popup saying game over and winner!");
        } 

    }


    // TODO fix this, but for now it will be hardcoded ot make all of the cards
    public List<Vector2> BuildDeck()
    {
        //Debug.Log("BuildDeck was called");

        var tempDeck = new List<Vector2>();
        var temp = new Vector2();
        int random;


        // TODO change these to 17-19 to include special cards
        for (int i = 1; i <= 15; i++)
        {
            for (int j = 1; j <= 15; j++)
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

        for (int i = 0; i < handSize; i++)
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


    public void Play()
    {   
        if (attackPanel.transform.childCount == 0 || defensePanel.transform.childCount == 0)
        {
            Debug.Log("Popup about proper number of cards");
            return;
        }

        Debug.Log("Play called");
        GetValues();

        var attDiff = myAtt - oppDef;
        var defDiff = oppAtt - myDef;

        //Debug.Log(string.Format("The att outcome was {0}, and the def outcome was {1}", attDiff, defDiff));

        if (attDiff > 0)
        {
            oppHealth -= attDiff;
        }
        if (defDiff > 0)
        {
            myHealth -= defDiff;
        }


        if (oppHealth <= 0 || myHealth <= 0 || handPanel.transform.childCount <= 1)
        {
            var count = handPanel.transform.childCount;

            round++;

            for (int i = 0; i < count; i++)
            {
                Destroy(handPanel.transform.GetChild(i).gameObject);
            }

            StartRound(myHealth, oppHealth);
        }

        Debug.Log("Popup to say what happened at the end of that hand.. animation would be nice too???");

        Destroy(attackPanel.transform.GetChild(0).gameObject);
        Destroy(defensePanel.transform.GetChild(0).gameObject);
        


    }


    public void GetValues()
    {

        // This feels Hacky!!!
        // Initialize attack and defense numbers of both player and opponent
        //attackPanel = GameObject.FindGameObjectWithTag("Attack Panel");
        //defensePanel = GameObject.FindGameObjectWithTag("Defense Panel");
        //Debug.Log(defensePanel);
        //Debug.Log(attackPanel);
        var attArray = attackPanel.GetComponentsInChildren<Text>();
        var defArray = defensePanel.GetComponentsInChildren<Text>();
        var oppAttArray = oppAttPanel.GetComponentsInChildren<Text>();
        var oppDefArray = oppDefPanel.GetComponentsInChildren<Text>();

        foreach (Text field in attArray)
        {
            if (field.name == "Attack")
            {
                //Debug.Log(field.text);
                myAtt = int.Parse(field.text);
            }
        }


        foreach (Text field in defArray)
        {
            if (field.name == "Defense")
            {
                myDef = int.Parse(field.text);
            }
        }


        foreach (Text field in oppAttArray)
        {
            if (field.name == "Attack")
            {
                oppAtt = int.Parse(field.text);
            }
        }


        foreach (Text field in oppDefArray)
        {
            if (field.name == "Defense")
            {
                oppDef = int.Parse(field.text);
            }
        }


       // Debug.Log(string.Format("myAtt = {0}, myDef = {1}, oppAtt = {2}, oppDef = {3}", myAtt, myDef, oppAtt, oppDef));
    }
}

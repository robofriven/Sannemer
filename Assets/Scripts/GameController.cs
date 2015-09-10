using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    [Header("Health and Score")]
    public int startingHealth = 20;
    [SerializeField]
    private int round;
    [SerializeField]
    private int myHealth;
    [SerializeField]
    private int oppHealth;
    [SerializeField]
    private int myScore;
    [SerializeField]
    private int oppScore;


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
    public Card oppAttCard;
    public Card oppDefCard;
    public GameObject cardBack1;
    public GameObject cardBack2;


    [HideInInspector]
    public List<Vector2> deck;
    [HideInInspector]
    public List<Vector2> hand;
    [HideInInspector]
    public Card card;
    private List<Vector2> AIDeck;
    private List<Vector2> AIHand;


    private int myAtt;
    private int myDef;
    private int oppAtt;
    private Vector2 oppAttValues;
    private int oppDef;
    private Vector2 oppDefValues;
    private Console console;
    private bool listeningForClick;
    private int typesOfCards = 17;

    void Start()
    {
        // Initialize the deck and hand so we have something to add to
        deck = new List<Vector2>();
        hand = new List<Vector2>();
        AIDeck = new List<Vector2>();
        AIHand = new List<Vector2>();


        console = GameObject.FindObjectOfType<Console>() as Console;
        cardBack1.SetActive(true);
        cardBack2.SetActive(true);


        // Start a new round
        round = 0;

        //foreach (Vector2 vector in hand)
        //{
        //    print(string.Format("Vector is ({0}, {1})", vector.x, vector.y));
        //}

    }

    // This method will be used to initialize a new round
    // Will update the score, reset the health, reshuffle and redeal
    public void StartRound(int MYSCORE = 0, int OPPSCORE = 0, int attDam = 0, int defDam = 0)
    {
        string message;
        List<string> button = new List<string>();

        // Initialize game starting variables
        if (round == 0)
        {
            round++;
            message = "Play your cards and press the go button";
            deck = BuildDeck();
            hand = Deal(deck);
            AIInit();
            console.Display(message);
            console.ClearButtons();
            myHealth = startingHealth;
            oppHealth = startingHealth;
            myScore = 0;
            oppScore = 0;
        }
        else if (round <= 3 && round != 0)
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
            hand = Deal(deck);
            AIHand = Deal(AIDeck);

            message = string.Format("You did {0} damage and received {1} damage. \nThe score is now {2} to {3}", attDam, defDam, myScore, oppScore);
            console.Display(message);
            console.ClearButtons();
        }
        else
        {
            button.Add("Again!");
            myScore += MYSCORE;
            oppScore += OPPSCORE;
            message = string.Format("You did {0} damage and received {1} damage. \nThe score is now {2} to {3}", attDam, defDam, myScore, oppScore);
            string msg;

            //Debug.Log("Popup saying game over and winner!");
            if (myScore > oppScore)
            {
                msg = "Game over, you Win! \n Press the button to play again.";
            }
            else if (myScore < oppScore)
            {
                msg = "Well played, but you lost this time. \nTry Again.";
            }
            else if (myScore == oppScore)
            {
                msg = "Well, you tied, and I don't know how to handle that yet, so push the button to play again.";
            }
            else
            {
                msg = "You didn't win lose or draw... stop cheating.";
            }

            message = message + "\n" + msg;
            console.Display(message, button);
            round = 0;
            console.buttons[0].onClick.AddListener(() => StartRound());
        }
    }


    // TODO fix this, but for now it will be hardcoded ot make all of the cards
    public List<Vector2> BuildDeck()
    {
        //Debug.Log("BuildDeck was called");

        var tempDeck = new List<Vector2>();
        var temp = new Vector2();
        int random;


       // the variable typesOfCards holds how many kinds of cards there are (right now it's 15 #s and 2 specials)
        for (int i = 1; i <= typesOfCards; i++)
        {
            for (int j = 1; j <= typesOfCards; j++)
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


    public List<Vector2> Deal(List<Vector2> deck)
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
            //current.AssignCard(temp[i]);
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

        //Debug.Log("Play called");
        GetValues();

        var attDiff = myAtt - oppDef;
        var defDiff = oppAtt - myDef;

        //Debug.Log(string.Format("The att outcome was {0}, and the def outcome was {1}", attDiff, defDiff));

        if (attDiff > 0)
        {
            oppHealth -= attDiff;
        }
        else
            attDiff = 0;
        if (defDiff > 0)
        {
            myHealth -= defDiff;
        }
        else
            defDiff = 0;

        string message = string.Format("You did {0} damage and received {1} damage. \nThe score is now {2} to {3}", attDiff, defDiff, myScore, oppScore);
        console.Display(message);


        Debug.Log(string.Format("oppHealth = {0}, myHealth = {1}, childCount = {2}", oppHealth, myHealth, handPanel.transform.childCount));

        if (oppHealth <= 0 || myHealth <= 0 || handPanel.transform.childCount <= 1)
        {
            var count = handPanel.transform.childCount;

            round++;

            for (int i = 0; i < count; i++)
            {
                Destroy(handPanel.transform.GetChild(i).gameObject);
            }

            StartRound(myHealth, oppHealth, attDiff, defDiff);
        }

        //Debug.Log("Popup to say what happened at the end of that hand.. animation would be nice too???");

        // Turn on listening for click and turn the card backs off
        listeningForClick = true;
        cardBack1.SetActive(false);
        cardBack2.SetActive(false);
        
    }


    public void GetValues()
    {
        // These are for determining if you or opponent will continue to figure out values (in case of specials)
        // This is the most cumbersome code I've written for this so far... would love to find a GOOD way to do this,
        // assuming that this way works at all that is.
        bool myAttDone = false;
        bool myDefDone = false;
        bool oppAttDone = false;
        bool oppDefDone = false;

        // Initialize attack and defense numbers of both player and opponent
        Debug.Log(attackPanel.GetComponentInChildren<Card>().attack);
        Debug.Log(defensePanel.GetComponentInChildren<Card>().defense);
        Debug.Log(oppAttPanel.GetComponentInChildren<Card>().attack);
        Debug.Log(oppDefPanel.GetComponentInChildren<Card>().defense);
        myAtt = attackPanel.GetComponentInChildren<Card>().attack;
        myDef = defensePanel.GetComponentInChildren<Card>().defense;

        // Right now I always have these cards available so there is no need to check to see if this is right.  When I instantiate this later
        // I can uncomment and fix this.
        //oppAtt = oppAttPanel.GetComponentInChildren<Card>().attack;
        //oppDef = oppDefPanel.GetComponentInChildren<Card>().defense;

        oppAtt = oppAttCard.attack;
        oppDef = oppDefCard.defense;

        // Covers the 'A' Case
        if (myAtt == 16 && !myAttDone)
        { 
            if (oppAtt < 16)
            {
                myAtt = oppAtt;
            }
            // If both players play 'A' (Only need to check once)
            else if (oppAtt == 16)
            {
                myAtt = 1;
                oppAtt = 1;
                oppAttDone = true;
            }
            // If one player plays 'A' and the other plays 'B'
            else if (oppAtt == 17)
            {
                myAtt = card.S2BaseAttack;

                if (myDef < 16)
                {
                    myDef += card.S2PlusDefense;
                    myDefDone = true;
                }
                else
                {
                    myDef = 1;
                    myDefDone = true;
                }

            }
        }


        if (myAtt == 17 && !myAttDone)
        {
            myAtt = card.S2BaseAttack;

            if (myDef < 16)
            {
                myDef += card.S2PlusDefense;
                myDefDone = true;
            }
            else
            {
                myDef = 1;
                myDefDone = true;
            }

        }

        if (oppAtt == 16 && !oppAttDone)
        {
            if (myAtt < 16)
            {
                oppAtt = myAtt;
            }
            else if (myAtt == 17)
            {
                oppAtt = card.S2BaseAttack;
                oppDef += card.S2PlusDefense;
            }
        }

        if (oppAtt == 17 && !oppAttDone)
        {
            oppAtt = card.S2BaseAttack;

            if (oppDef < 16)
            {
                oppDef += card.S2PlusDefense;
                oppDefDone = true;
            }
            else
            {
                oppDef = 1;
                oppDefDone = true;
            }
        }


        if (myDef == 16 && !myDefDone)
        {
            if (oppDef < 16)
            {
                myDef = oppDef;
            }
            // If both players play 'A' (Only need to check once)
            else if (oppDef == 16)
            {
                myDef = 1;
                oppDef = 1;
                oppDefDone = true;
            }
            // If one player plays 'A' and the other plays 'B'
            else if (oppDef == 17)
            {
                myDef = oppDef;
            }
        }

        if (myDef == 17)
        {
            myDef = oppAtt;
        }

        if (oppDef == 16)
        {
            if (oppDef < 16)
            {
                oppDef = myDef;
            }

            // If one player plays 'A' and the other plays 'B'
            else if (oppDef == 17)
            {
                oppDef = myDef;
            }
        }
        if (oppDef == 17)
        {
            oppDef = myAtt;
        }


        // Debug.Log(string.Format("myAtt = {0}, myDef = {1}, oppAtt = {2}, oppDef = {3}", myAtt, myDef, oppAtt, oppDef));
    }

    void Update()
    {

        // when listening for clicks is on it will listen for a click (original I know) and then turn card backs back on and calls startRound
        if (listeningForClick)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                listeningForClick = false;
                cardBack1.SetActive(true);
                cardBack2.SetActive(true);
                Destroy(attackPanel.transform.GetChild(0).gameObject);
                Destroy(defensePanel.transform.GetChild(0).gameObject);
            }
        }
    }


    // Initializes AI (builds the deck, deals the hand, sets the attack/defense bias and orders the hand
    private void AIInit()
    {
        AIDeck = BuildDeck();
        AIHand = Deal(AIDeck);

        // Decide whether it will be attack or Defense biased
        var random = Random.Range(0, 2);
        print("Random number is " + random);
        var bias = string.Empty;

        if (random == 0)
        {
            bias = "attack";
        }
        if (random == 1)
        {
            bias = "defense";
        }
        else
        {
            Debug.Log("Something screwed up randomly!");
            throw new System.NotImplementedException();
        }

        AIHand = Order(bias);
    }
    private Vector2[] AIFindPlay()
    {
        var random = Random.Range(0, 2);
        oppAttValues = Vector2.zero;
        oppDefValues = Vector2.zero;


        switch (random)
        {
            // Style = best
            case 0:
                if (AIHand[0] == AIHand[1])
                {
                    oppDefValues = AIHand[1];
                    oppAttValues = AIHand[0];
                }
                else if (AIHand[0].x > AIHand[1].x)
                {
                    oppAttValues = AIHand[0];
                    AIHand.RemoveAt(0);
                }
                else if (AIHand[0].x < AIHand[1].x || AIHand[0].y > AIHand[1].y)
                {
                    oppAttValues = AIHand[1];
                    oppDefValues = AIHand[0];
                }
                else
                {
                    oppAttValues = AIHand[0];
                    oppDefValues = AIHand[1];
                }
                AIHand.RemoveAt(0);
                AIHand.RemoveAt(1);
                break;

            // Style = Worst
            case 1:
                if (AIHand[AIHand.Count] == AIHand[AIHand.Count - 1])
                {
                    oppDefValues = AIHand[AIHand.Count - 1];
                    oppAttValues = AIHand[AIHand.Count];
                }
                else if (AIHand[AIHand.Count].x < AIHand[AIHand.Count - 1].x)
                {
                    oppAttValues = AIHand[AIHand.Count];
                    AIHand.RemoveAt(AIHand.Count);
                }
                else if (AIHand[AIHand.Count].x > AIHand[AIHand.Count - 1].x || AIHand[AIHand.Count].y < AIHand[AIHand.Count - 1].y)
                {
                    oppAttValues = AIHand[AIHand.Count - 1];
                    oppDefValues = AIHand[AIHand.Count];
                }
                else
                {
                    oppAttValues = AIHand[AIHand.Count];
                    oppDefValues = AIHand[AIHand.Count - 1];
                }
                AIHand.RemoveAt(AIHand.Count);
                AIHand.RemoveAt(AIHand.Count);
                break;
            default:
                Debug.Log("Something screwed up randomly!");
                throw new System.NotImplementedException();
        }

        var play = new Vector2[2] { oppAttValues, oppDefValues };
        return play;
    }

   public List<Vector2> Order(string bias)
    {
        Vector2 tmpAtt = new Vector2();
        Vector2 tmpDef = new Vector2();
        List<Vector2> tmpHand = new List<Vector2>();

        if (bias == "attack")
        {
            while (AIHand.Count > 0)
            {
                foreach (Vector2 card in AIHand)
                {
                    if (card.x > tmpAtt.x)
                    {
                        tmpAtt = card;
                    }
                }

                tmpHand.Add(tmpAtt);
                AIHand.Remove(tmpAtt);

                foreach (Vector2 card in AIHand)
                {
                    if (card.y > tmpDef.y)
                    {
                        tmpDef = card;
                    }
                }
                tmpHand.Add(tmpDef);
                AIHand.Remove(tmpDef);
            }
        }
        else if (bias == "defense")
        {
            while (AIHand.Count > 0)
            {
                foreach (Vector2 card in AIHand)
                {
                    if (card.y > tmpDef.y)
                    {
                        tmpDef = card;
                    }
                }
                tmpHand.Add(tmpDef);
                AIHand.Remove(tmpDef);

                foreach (Vector2 card in AIHand)
                {
                    if (card.x > tmpAtt.x)
                    {
                        tmpAtt = card;
                    }
                }
                tmpHand.Add(tmpAtt);
                AIHand.Remove(tmpAtt);
            }
        }
        else
        {
            Debug.Log("Determine is broke");
            throw new System.NotImplementedException();
        }
        return tmpHand;
    }
}



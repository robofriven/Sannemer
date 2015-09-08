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

    private int myAtt;
    private int myDef;
    private int oppAtt;
    private int oppDef;
    private Console console;

    void Start()
    {
        // Initialize the deck and hand so we have something to add to
        deck = new List<Vector2>();
        hand = new List<Vector2>();
        

        console = GameObject.FindObjectOfType<Console>() as Console;


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

        if (round == 0)
        {
            round++;
            message = "Play your cards and press the go button";
            deck = BuildDeck();
            hand = Deal(deck);
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

            message = string.Format("You did{0} damage and received {1} damage. \nThe score is now {2} to {3}", attDam, defDam, myScore, oppScore);
            console.Display(message);
            console.ClearButtons();
        }
        else
        {
            button.Add("Again!");
            myScore += MYSCORE;
            oppScore += OPPSCORE;
            message = string.Format("You did{0} damage and received {1} damage. \nThe score is now {2} to {3}", attDam, defDam, myScore, oppScore);
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

        var oppCards = buildAIDeck();
        int random;

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

        random = (int)Random.Range(0, oppCards.Count);

        foreach (Text field in oppAttArray)
        {
            if (field.name == "Attack")
            {
                oppAtt = (int)oppCards[random].x;
                field.text = oppAtt.ToString();
            }

            if (field.name == "Defense")
            {
                field.text = oppCards[random].y.ToString();
            }
        }
        oppCards.RemoveAt(random);

        random = Random.Range(0, oppCards.Count);

        foreach (Text field in oppDefArray)
        {
            if (field.name == "Defense")
            {
                Debug.Log(string.Format("oppCards.Count = {0}, and random = {1}", oppCards.Count, random));
                oppDef = (int)oppCards[random].y;
                field.text = oppDef.ToString();
            }
            if (field.name == "Attack")
            {
                field.text = oppCards[random].y.ToString();                
            }
        }
        oppCards.RemoveAt(random);


        // Debug.Log(string.Format("myAtt = {0}, myDef = {1}, oppAtt = {2}, oppDef = {3}", myAtt, myDef, oppAtt, oppDef));
    }

    List<Vector2> buildAIDeck()
    {
        List<Vector2> AIhand = new List<Vector2>();
        for (int i = 0; i < handSize; i++)
        {
            AIhand.Add(new Vector2((int)Random.Range(1, 16), (int)Random.Range(1, 16)));
        }

        return AIhand;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultiController : MonoBehaviour
{

    // Public variables and fields
    public int ready = 0;

    [Header("Player info")]
    public int startingHealth = 20;
    public int DeckSize = 30;
    public int handSize = 6;
    public int round = 0;
    public int myHealth;
    public int oppHealth;
    public int myScore;
    public int oppScore;
    public Image myhealthbar;
    public Image oppHealthbar;

    [Header("Card Values")]
    public int myAttack;
    public int oppAttack;
    public int myDefense;
    public int oppDefense;
    public int unusedAttack;
    public int unusedDefense;
    [Space(10)]
    public int numCardTypes = 17;

    [Header("Fields")]
    public Console console;
    public GameObject attackField;
    public GameObject defenseField;
    public Card oppAttCard;
    public Card oppDefCard;
    public GameObject cardBack1;
    public GameObject cardBack2;
    public Text myScoreField;
    public Text oppScoreField;

    [Header("Prefabs")]
    public Card cardPrefab;

    [HideInInspector]
    public bool goOff = false;

    // Private Variables
    private Hand hand;
    private PhotonView photonView;
    private ButtonHandler buttonHandler;
    private bool PlayersReady           =     false;
    private bool RoundReady             =     false;
    private bool waitForClick           =     false;
    private bool sent                   =     false;
    private bool oppSent                =     false;
    private int roundReady              =       0;
    private int amountReady             =       0;
    


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        buttonHandler = GetComponent<ButtonHandler>();
    }

    public void Ready()
    {
        photonView.RPC("Intermediate", PhotonTargets.All);
        //Debug.Log("The game should start");
        console.Display("The game should begin shortly...");
    }

    [PunRPC]
    public void Intermediate()
    {
        ready++;
    }



    void Update()
    {
        //Debug.Log(PlayersReady);
        if (!PlayersReady)
        {
            if (ready == 2)
            {
                PlayersReady = true;
                if (PhotonNetwork.isMasterClient)
                {
                    photonView.RPC("StartGame", PhotonTargets.All);
                }
            }
        }

        if (!RoundReady)
        {
            if (roundReady == 2)
            {
                RoundReady = true;
                if (PhotonNetwork.isMasterClient)
                {
                    photonView.RPC("Play", PhotonTargets.All);
                }
                
            }
        }

        if (waitForClick)
        {
            if (Input.GetButton("Fire1"))
            {
                waitForClick = false;
                NextHand();
            }
        }
        if (!sent)
        {
            print(amountReady);
            if (amountReady == 2)
            {
                sent = true;
                print("Outcome is being called now");
                Outcome();
            }
        }
    }

    [PunRPC]
    private void StartGame()
    {
        InitGame();
    }

    private void InitGame()
    {
        round = 0;
        myScore = 0;
        oppScore = 0;
        myHealth = startingHealth;
        oppHealth = startingHealth;
        hand = new Hand(DeckSize, handSize, numCardTypes, cardPrefab);
        CardBacks(true);
    }

    // Called from hitting the go Button, calls the intermediate method to increment roundReady, when both players add
    // one to roundReady it triggers Play to be called
    public void CardsReady()
    {
        photonView.RPC("RoundIntermediate", PhotonTargets.All);
        //Debug.Log("Player is ready!!!");
        console.Display("You are locked in, waiting for your opponent");
        console.ClearButtons();
    }

    [PunRPC]
    public void RoundIntermediate()
    {
        roundReady++;
    }


    // Called from roundReady equalling 2.  Sets values of cards and then triggers the amountsent method which will
    // send the values to the other player populating the opponent cards and incrementing amountReady which
    // means that both players have sent their values to the opponent which will trigger the Outcome method
    [PunRPC]
    public void Play()
    {
        Debug.Log("Play is called");
        console.Display("You are locked in, waiting for your opponent");
        console.ClearButtons();

        myAttack = attackField.GetComponentInChildren<Card>().attack;
        myDefense = defenseField.GetComponentInChildren<Card>().defense;
        var unusedA = attackField.GetComponentInChildren<Card>().defense;
        var unusedD = defenseField.GetComponentInChildren<Card>().attack;
        photonView.RPC("amountSent", PhotonTargets.Others, myAttack, myDefense, unusedA, unusedD);
        amountReady++;
       

    }

    [PunRPC]
    private void amountSent(int att, int def, int unusedA, int unusedD)
    {
        oppAttack = att;
        oppDefense = def;
        unusedAttack = unusedA;
        unusedDefense = unusedD;

        amountReady++;
    }

    // triggered from both players sending their card values.  Determines/displays damage and starts the "wait for click" state.  After click then the NextHand method is called.
    private void Outcome()
    {
        int myDamage = myAttack - oppDefense;
        int oppDamage = oppAttack - myDefense;

        if (myDamage < 0)
            myDamage = 0;
        if (oppDamage < 0)
            oppDamage = 0;

        myHealth -= oppDamage;
        oppHealth -= myDamage;
        myhealthbar.fillAmount = ((float)myHealth / (float)startingHealth);
        oppHealthbar.fillAmount = ((float)oppHealth / (float)startingHealth);

        oppAttCard.BuildCard(oppAttack, unusedAttack);
        oppDefCard.BuildCard(unusedDefense, oppDefense);
        CardBacks(false);

        string message = string.Format("You did {0} damage. \nYour opponent did {1} damage. \nYour health is {2}, while his health is {3}. \nClick to continue.", myDamage, oppDamage, myHealth, oppHealth);
        console.Display(message);
        console.ClearButtons();
        

        waitForClick = true;
    }


    // triggered after the opponent clicks.  It resets all of the values for hand, destroys the played cards and checks
    // to see if the round is over. (Which calls NextRound())
    private void NextHand()
    {
        print("Nexthand called");
        CardBacks(true);

        amountReady = 0;
        roundReady = 0;
        RoundReady = false;
        sent = false;
        goOff = false;           

        Destroy(attackField.GetComponentInChildren<Card>().gameObject);
        Destroy(defenseField.GetComponentInChildren<Card>().gameObject);

        Debug.Log("Hand count is " + hand.handPanel.transform.childCount);
        if (myHealth < 1 || oppHealth < 1 || hand.handPanel.transform.childCount <= 1)
        {
            NextRound();
        }
    }

    // Called from NextHand if the round is over.  Resets health and determines score and dtermines whether the game is over
    private void NextRound()
    {
        Debug.Log("NextRound Called");
        round++;

        if (myHealth < 0)
            myHealth = 0;
        if (oppHealth < 0)
            oppHealth = 0;

        myScore += myHealth;
        oppScore += oppHealth;
        myScoreField.text = "Score: " + myScore.ToString();
        oppScoreField.text = "Score: " + oppScore.ToString();

        myHealth = startingHealth;
        oppHealth = startingHealth;

        var hnd = hand.handPanel.GetComponentsInChildren<Card>();

        foreach (Card card in hnd)
        {
            Destroy(card.gameObject);
        }

        console.Display(string.Format("Round over. \nYour score is: {0} \nOpponent's score is: {1}", myScore, oppScore));

        hand.hand = hand.Deal(hand.deck);

        if (round > 2)
        {
            if (myScore > oppScore)
            {
                console.Display(string.Format("You win! Well played. \nThe score was {0} to {1}. \nPress the button to play again", myScore, oppScore), new List<string> { "Again!"});
                console.buttons[0].onClick.AddListener(() => buttonHandler.StartMulti());
            }
            if (oppScore > myScore)
            {
                console.Display(string.Format("You Lost. The score was {0} to {1}. \nPress the button to play again.", oppScore, myScore), new List<string> { "Again" });
                console.buttons[0].onClick.AddListener(() => buttonHandler.StartMulti());
            }
            if (oppScore == myScore)
            {
                console.Display(string.Format("It was a tie! {0}-{1} \nPress the button for a rematch.", myScore, oppScore), new List<string> { "Again" });
                console.buttons[0].onClick.AddListener(() => buttonHandler.StartMulti());
            }
        }
    }

    // turns on or off the card backs
    private void CardBacks(bool show)
    {
        cardBack1.SetActive(show);
        cardBack2.SetActive(show);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}

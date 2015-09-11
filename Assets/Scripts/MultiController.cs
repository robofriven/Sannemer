using UnityEngine;
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
    public int OppScore;
    [Header("Card Values")]
    public int myAttack;
    public int oppAttack;
    public int myDefense;
    public int oppDefense;
    [Space(10)]
    public int numCardTypes = 17;
    [Header("Fields")]
    public Console console;
    public GameObject attackField;
    public GameObject defenseField;
    [Header("Prefabs")]
    public Card cardPrefab;

    // Private Variables
    private Hand hand;
    private PhotonView photonView;
    private bool PlayersReady           =     false;
    private bool RoundReady             =     false;
    private bool cardsReady             =     false;
    private bool waitForClick           =     false;
    private bool cardsSent              =     false;
    private bool cardsReceived          =     false;
    private int roundReady              =       0;


    void Start()
    {
        photonView = GetComponent<PhotonView>();
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
                NextRound();
            }
        }

        //Debug.Log(cardsSent);
        //Debug.Log(cardsReceived);
        //if (cardsReceived)
        //{
        //    Outcome();
        //}

        if (myAttack != 0 && myDefense != 0 && oppAttack != 0 && oppDefense != 0)
        {
            Outcome();
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
        OppScore = 0;
        myHealth = startingHealth;
        oppHealth = startingHealth;
        hand = new Hand(DeckSize, handSize, numCardTypes, cardPrefab);
    }


    public void CardsReady()
    {
        photonView.RPC("RoundIntermediate", PhotonTargets.All);
        //Debug.Log("Player is ready!!!");
        console.Display("This is not firing correctly");
        console.ClearButtons();
    }

    [PunRPC]
    public void RoundIntermediate()
    {
        roundReady++;
    }


    [PunRPC]
    public void Play()
    {
        Debug.Log("Play is called");
        console.Display("Play logic will soon commence");

        myAttack = attackField.GetComponentInChildren<Card>().attack;
        myDefense = defenseField.GetComponentInChildren<Card>().defense;
        cardsReady = true;
       

    }

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


        string message = string.Format("You did {0} damage. \nYour opponent did {1} damage. \nYour health is {2}, while his health is {3}. \nClick to continue.", myDamage, oppDamage, myHealth, oppHealth);
        console.Display(message);
        console.ClearButtons();
        

        waitForClick = true;
    }

    private void NextRound()
    {
        Debug.Log("NextRound Called");
        round++;
        if (round > 2)
        {
            // Do whatever it takes to end the game
            // Popup to play another
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (cardsReady)
        {
            if (stream.isWriting)
            {
                //stream.SendNext(thing);
                stream.SendNext(myAttack);
                stream.SendNext(myDefense);

            }
            if (stream.isReading)
            {
                //thing = stream.ReceiveNext();
                oppAttack = (int)stream.ReceiveNext();
                oppDefense = (int)stream.ReceiveNext();
                cardsReceived = true;
            }
        }
    }

}

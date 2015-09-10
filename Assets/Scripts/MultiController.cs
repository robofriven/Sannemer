using UnityEngine;
using System.Collections.Generic;

public class MultiController : MonoBehaviour {

    // Public variables and fields
    public int ready = 0;
    public bool PlayersReady = false;

    [Header("Player info")]
    public int startingHealth = 20;
    public int DeckSize = 30;
    public int handSize = 6;
    public int round = 0;
    public int myHealth;
    public int oppHealth;
    public int myScore;
    public int OppScore;
    [Space(10)]
    public int numCardTypes = 17;
    [Header("Fields")]
    public Console console;
    [Header("Prefabs")]
    public Card cardPrefab;

    // Private Variables
    private Hand hand;
    private PhotonView photonView;

    
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
        if (!PlayersReady)
        {
            if (ready == 2)
            {
                PlayersReady = true;
                photonView.RPC("StartGame", PhotonTargets.All);                
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
        OppScore = 0;
        myHealth = startingHealth;
        oppHealth = startingHealth;
        hand = new Hand(DeckSize, handSize, numCardTypes, cardPrefab);
    }
}

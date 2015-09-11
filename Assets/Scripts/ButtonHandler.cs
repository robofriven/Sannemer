using UnityEngine;
using System.Collections;
using System;

public class ButtonHandler : MonoBehaviour
{
    public GameController gameController;
    public Console console;
    public MultiController multiController;
    public PhotonView photonView;
   

    void Start()
    {
        gameController = GetComponent<GameController>();
        console = GameObject.FindObjectOfType<Console>() as Console;
        multiController = GetComponent<MultiController>();
    }

    // Button that says both players are ready
    public void GoButton()
    {
        //gameController.Play();
        multiController.CardsReady();


    }

    internal void StartGame()
    {
        string message = "Play your cards and press the go button";
        console.Display(message);
        console.ClearButtons();
        gameController.StartRound();
    }

    internal void StartMulti()
    {
        Debug.Log("The game would start");
        console.Display("The game should begin shortly...");
        multiController.Ready();
        console.ClearButtons();   
    }
}

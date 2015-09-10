using UnityEngine;
using System.Collections;
using System;

public class ButtonHandler : MonoBehaviour
{
    public GameController gameController;
    public Console console;
    public MultiController multiController;
   

    void Start()
    {
        gameController = GetComponent<GameController>();
        console = GameObject.FindObjectOfType<Console>() as Console;
    }

    public void GoButton()
    {
        gameController.Play();
    }

    internal void StartGame()
    {
        string message = "Play your cards and press the go button";
        console.Display(message);
        gameController.StartRound();
    }

    internal void StartMulti()
    {
        Debug.Log("The game would start");
        console.Display("The game should begin shortly...");
        multiController.Ready();        
    }
}

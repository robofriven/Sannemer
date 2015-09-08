using UnityEngine;
using System.Collections;
using System;

public class ButtonHandler : MonoBehaviour
{
    public GameController gameController;
    public Console console;

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
}

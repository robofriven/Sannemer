using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour
{
    public GameController gameController;

    void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void GoButton()
    {
        gameController.Play();
    }

}

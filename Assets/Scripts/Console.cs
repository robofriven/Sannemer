using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Console : MonoBehaviour
{
    [Header("Fields/Prefabs")]
    public GameObject buttonField;
    public GameObject textField;
    public Button button; 

    [TextArea(3, 5)]
    public string message;
    [HideInInspector]
    public List<Button> buttons;

    private Typer typer;
    private GameController gameController;
    private ButtonHandler buttonHandler;


    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>() as GameController;
        buttonHandler = GameObject.FindObjectOfType<ButtonHandler>() as ButtonHandler;
        typer = GetComponent<Typer>();
        buttons = new List<Button>();

        var names = new List<string> { "Start!" };

        Display(message, names);
        buttons[0].onClick.AddListener(() => buttonHandler.StartGame());
    }


    public void Display (string message, List<string> buttonNames = null)
    {
        textField.GetComponent<Text>().text = message;

        if (buttonNames != null && buttonNames.Count != 0)
        {
            for (int i = 0; i < buttonNames.Count; i++)
            {
                //Debug.Log(buttonNames.Count);

                // instantiate the buttons
                buttons.Add(GameObject.Instantiate<Button>(button));
                buttons[i].transform.SetParent(buttonField.transform);
                buttons[i].transform.GetChild(0).GetComponent<Text>().text = buttonNames[i];
            }
        }
    }

    public void ClearButtons()
    {
        foreach (Button butt in buttons)
        {
            Destroy(butt.gameObject);
        }
        buttons.Clear();
    }
}

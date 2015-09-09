using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Stats")]
    new string name;
    public int attack;
    public int defense;
    public string attackFlavor;
    public string defenseFlavor;

    [Header("Special Card Variables")]
    public int S2BaseAttack;
    public int S2PlusDefense;

    // private variables that won't show in inspector
    private Text attackText;
    private Text defenseText;
    private Text attackFlavorText;
    private Text defenseFlavorText;


    // Possible additions
    // value
    // Maybe collection/set number
    // rarity

    void Awake()
    {
        // Initializiing private variables to text fields
        //Text[] textArray = new Text[4];
        Text[] textArray = GetComponentsInChildren<Text>();
        foreach (Text field in textArray)
        {
            if (field.name == "Attack")
            {
                attackText = field;
            }
            else if (field.name == "Defense")
            {
                defenseText = field;
            }
            else
            { print("Text doesn't go to field"); }
        }
    }

    public void AssignCard(Vector2 stats)
    {


        if (attackText != null && defenseText != null)
        {

            if (stats.x < 16)
            {
                attack = (int)stats.x;
                attackText.text = stats.x.ToString();
            }
            else if (stats.x == 16)
            {
                attack = (int)stats.x;
                attackText.text = "A";
            }
            else if (stats.x == 17)
            {
                attack = (int)stats.x;
                attackText.text = "B";
            }
            else
            {
                Debug.Log(string.Format("X failed, Vector is ({0}, {1})", stats.x, stats.y));
                throw new System.NotImplementedException();
            }


            if (stats.y < 16)
            {
                defense = (int)stats.y;
                defenseText.text = stats.y.ToString();
            }
            else if (stats.y == 16)
            {
                defense = (int)stats.y;
                defenseText.text = "A";
            }
            else if (stats.y == 17)
            {
                defense = (int)stats.y;
                defenseText.text = "B";
            }
            else
            {
                Debug.Log(string.Format("Y failed, Vector is ({0}, {1})", stats.x, stats.y));
                throw new System.NotImplementedException();
            }
        }
    }
}

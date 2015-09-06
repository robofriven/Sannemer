using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Stats")]
    new string name;
    public int attack;
    public int defense;
    public string attackFlavor;
    public string defenseFlavor;

    // private variables that won't show in inspector
    private Text attackText;
    private Text defenseText;
    private Text attackFlavorText;
    private Text defenseFlavorText;


    // Possible additions
    // sprite (image)
    // value
    // Maybe collection/set number
    // rarity
    // classes/races

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
                attackText.text = stats.x.ToString();
            }
            else if (stats.x == 16)
            {
                attackText.text = "A";
            }
            else if (stats.x == 17)
            {
                attackText.text = "B";
            }
            else
            {
                Debug.Log(string.Format("X failed, Vector is ({0}, {1})", stats.x, stats.y));
                throw new System.NotImplementedException();
            }


            if (stats.y < 16)
            {
                defenseText.text = stats.y.ToString();
            }
            else if (stats.y == 16)
            {
                defenseText.text = "A";
            }
            else if (stats.y == 17)
            {
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

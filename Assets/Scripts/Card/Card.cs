using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Stats")]
    public int attack;
    public int defense;
    public string attackFlavor;
    public string defenseFlavor;


    [Header("Special Card Variables")]
    public Specials offSpecial;
    public Specials defSpecial;
    public int S2BaseAttack;
    public int S2PlusDefense;

    // private variables that won't show in inspector
    private Text attackText;
    private Text defenseText;
    private Text attackFlavorText;
    private Text defenseFlavorText;

    [HideInInspector]
    public enum Specials
    {
        NONE,
        AttackSteal,
        DefenseSteal,
        AttackPlusOne,
        DefensePlusOne
    }
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

    public void BuildCard(int attack, int defense, Specials offSpecial = Specials.NONE, Specials defSpecial = Specials.NONE)
    {
        this.attack = attack;
        attackText.text = attack.ToString();
        this.defense = defense;
        defenseText.text = defense.ToString();

        if (offSpecial != Specials.NONE)
            this.offSpecial = offSpecial;
        if (defSpecial != Specials.NONE)
            this.defSpecial = defSpecial;
    }

}

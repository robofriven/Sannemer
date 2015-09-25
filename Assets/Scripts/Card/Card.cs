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

    [Header("Prefabs/Fields")]
    public Card cardPrefab;

    // private variables that won't show in inspector
    public Text attackText;
    public Text defenseText;
    private Text attackFlavorText;
    private Text defenseFlavorText;
    private GameObject deckSpot;


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
        deckSpot = GameObject.FindGameObjectWithTag("Deck Layout");

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

    public Card AssembleCard(Card card, int attack, int defense, Specials offSpecial = Specials.NONE, Specials defSpecial = Specials.NONE)
    {
        card.transform.SetParent(deckSpot.transform);
        card.attack = attack;
        attackText.text = attack.ToString();
        card.defense = defense;
        defenseText.text = defense.ToString();

        card.gameObject.GetComponent<Draggable>().enabled = false;

        if (offSpecial != Specials.NONE)
            card.offSpecial = offSpecial;
        if (defSpecial != Specials.NONE)
            card.defSpecial = defSpecial;

        return card;
    }

    public static void DestroyCards()
    {
        GameObject deckSpot = GameObject.FindGameObjectWithTag("Deck Layout");
        for (int i = 0; i < deckSpot.transform.childCount; i++)
        {
            Destroy(deckSpot.transform.GetChild(i).gameObject);
        }
    }

}

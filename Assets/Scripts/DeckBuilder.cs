using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public Card cardPrefab;

    [Space(10)]
    public Text attackText;
    public GameObject attackSpot;
    public Text defenseText;
    public GameObject defenseSpot;

    private List<Card> deck;
    private Vector2[] deckList;
    private int deckIndex = 0;

    void Start()
    {
        deck = new List<Card>();
        deckList = PlayerPrefsX.GetVector2Array("Deck");

        if (deckList != null && deckList[4] != Vector2.zero)
        {
            RetrieveDeck();
        }
        else deckList = new Vector2[30];
    }

    public void AddToDeck()
    {
        if (attackSpot.transform.childCount == 0 && attackSpot.transform.childCount != 0 && defenseSpot.transform.childCount != 0)
        {
            Debug.Log("You need more gears!");
            return;
        }

        Debug.Log("Add to Deck");
        var card = Instantiate(cardPrefab);
        card.AssembleCard(card, int.Parse(attackText.text), int.Parse(defenseText.text));
        deck.Add(card);

        deckList[deckIndex] = new Vector2(card.attack, card.defense);
        deckIndex++;

        card.attackText.fontSize = 10;
        card.defenseText.fontSize = 10;

        LayoutElement le = card.GetComponent<LayoutElement>();
        le.preferredHeight = le.minHeight;
        le.preferredWidth = le.minWidth;


        if (attackSpot.transform.childCount != 0 && defenseSpot.transform.childCount != 0)
        {
            Destroy(attackSpot.transform.GetChild(0).gameObject);
            Destroy(defenseSpot.transform.GetChild(0).gameObject);
        }
        attackText.text = "";
        defenseText.text = "";
    }

    public void AddToDeck(Vector2[] list)
    {
        print("AddToDeck");
        foreach (Vector2 vector in deckList)
        {
            print("foreach loop");
            attackText.text = vector.x.ToString();
            defenseText.text = vector.y.ToString();
            AddToDeck();
        }
    }

    private void RetrieveDeck()
    {
        deckList = PlayerPrefsX.GetVector2Array("Deck");
        deckIndex = 0;

        foreach (Vector2 vector in deckList)
        {
            print(string.Format("x = {0}, y = {1}", vector.x, vector.y));
        }

        AddToDeck(deckList);
    }

    public void SaveDeck()
    {
        if (deckList[4] != Vector2.zero)
        {
            PlayerPrefsX.SetVector2Array("Deck", deckList);
        }
        else
            print("There is nothing to save");
    }

    public void DeleteDeck()
    {
        deckIndex = 0;
        for (int i = 0; i < deckList.Length; i++)
        {
            deckList[i] = Vector2.zero;
        }

        if (deckList[0] != null)
        {
            PlayerPrefsX.SetVector2Array("Deck", deckList);
            Card.DestroyCards();
        }
        else
        {
            print("There is nothing to delete");
        }
    }

    public void Play(int level)
    {
        Application.LoadLevel(level);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public Hand hand;

    public List<Card> AllCards;

    public int DeckSize = 15;

    private Stack<Card> _deck;

    private static System.Random rng = new System.Random();

    public void Draw()
    {
        if (_deck.Count == 0)
        {
            NewDeck();
        }

        var cardPrefab = _deck.Pop();

        if (cardPrefab)
        {
            var cardGameObject = Instantiate(cardPrefab, transform.position, transform.rotation);
            hand.AddCard(cardGameObject);
        }

        UpdateText();
    }

    void NewDeck()
    {
        var cards = new List<Card>();
        for (var i = 0; i < DeckSize; i++)
        {
            var allCardIndex = i % AllCards.Count;
            if (allCardIndex == 0)
            {
                AllCards.Shuffle();
            }
            cards.Add(AllCards[allCardIndex]);
        }
        _deck = new Stack<Card>(cards.Shuffle());
    }

    // Start is called before the first frame update
    void Start()
    {
        NewDeck();

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyUp(KeyCode.Space))
        // {
        //     Draw();
        // }
    }

    void UpdateText()
    {
        Text.text = string.Format("{0}/{1}", _deck.Count.ToString(), DeckSize);
    }
}

public static class ListExtensions
{
    // https://stackoverflow.com/questions/273313/randomize-a-listt
    private static System.Random rng = new System.Random();  

    public static List<T> Shuffle<T>(this List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }
        
        return list;
    }
}

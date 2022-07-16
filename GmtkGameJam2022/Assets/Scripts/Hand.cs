using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float CardSpacing = 5;

    private List<Card> _cards;

    public void AddCard(Card card)
    {
        _cards.Add(card);

        card.transform.parent = transform;

        card.transform.position = CalculateCardPosition(_cards.Count - 1, _cards.Count);
    }

    private Vector3 CalculateCardPosition(int index, int total)
    {
        // var totalWidth = total * CardSpacing;
        // var halfWidth = totalWidth / 2;
        // var position = index * CardSpacing;

        return new Vector3(
            index * CardSpacing,
            0,
            0
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // try calculating and lerping to arc for each card
    }
}

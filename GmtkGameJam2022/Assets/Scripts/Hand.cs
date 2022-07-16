using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // public float CardSpacing = 5;
    float MinAngle = -1;
    float MaxAngle = 1;
    float XScalar = 5;
    float YScalar = 2;

    private List<Card> _cards;

    public void AddCard(Card card)
    {
        _cards.Add(card);

        card.transform.parent = transform;

        for(var i = 0; i< _cards.Count; i++)
        {
           _cards[i].AnimateTo(CalculateCardPosition(i, _cards.Count));
        }
    }

    private Vector3 CalculateCardPosition(int index, int total)
    {
        var alpha = (float)(index + 1) / (float)(total + 1);

        var angle = Mathf.Lerp(MinAngle, MaxAngle, alpha);

        return new Vector3(
            Mathf.Sin(angle) * XScalar,
            Mathf.Cos(angle) * YScalar,
            -alpha
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        _cards = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

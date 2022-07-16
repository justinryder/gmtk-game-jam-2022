using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    // public float CardSpacing = 5;
    public float MinAngle = -1;
    public float MaxAngle = 1;
    public float XScalar = 5;
    public float YScalar = 2;

    private List<Card> _cards;

    public void AddCard(Card card)
    {
        _cards.Add(card);

        card.transform.SetParent(transform);
        card.transform.rotation = Quaternion.identity;
        card.GetComponent<Button>().onClick.AddListener(() => HandleCardClick(card));

        for(var i = 0; i< _cards.Count; i++)
        {
           _cards[i].AnimateTo(CalculateCardPosition(i, _cards.Count));
        }
    }

    void HandleCardClick(Card card)
    {
        Debug.Log("Clicked card" + card.cardData.Action);
        card.Play();

        _cards.Remove(card);
        card.AnimateToThenDestroy(Vector3.forward * -1000);
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

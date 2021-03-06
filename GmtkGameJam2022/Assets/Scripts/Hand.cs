using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hand : MonoBehaviour
{
    // public float CardSpacing = 5;
    public float MinAngle = -1;
    public float MaxAngle = 1;
    public float XScalar = 5;
    public float YScalar = 2;

    public HealthBarController healthBar;
    public HealthBarController timeBar;
    public TextMeshProUGUI messageText;

    public Transform DiscardTarget;
    public Transform SelectedTarget;

    public Button NextEncounterButton;

    public DiceRoller DiceRoller;

    private List<Card> _cards;

    private static System.Random rng = new System.Random();

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

    public void Discard()
    {
        while (_cards.Count > 0)
        {
            Discard(_cards[0]);
        }
    }

    public void Discard(Card card)
    {
        card.AnimateToThenDestroy(DiscardTarget.localPosition);
        card.AnimateToScale(Vector3.one);
        _cards.Remove(card);
    }

    void HandleCardClick(Card card)
    {
        Debug.Log("Playing card " + card.cardData.Action);

        var encounterGameObject = GameObject.FindWithTag("Encounter");
        
        if (!encounterGameObject)
        {
            Debug.Log("No Encounter tagged object in scene.");
            return;
        }
        
        var encounter = encounterGameObject.GetComponent<Encounter>();

        messageText.text = string.Format("{0}...\nRolling to beat a 3 with a {1} modifier...", card.cardData.PlayString.Replace("%E", encounter.encounterData.Name), card.cardData.Bonus.ToSignedString());

        DiceRoller.Roll((int value) => {
            value += 1;
            var success = value + card.cardData.Bonus > 3;

            var result = encounter.GetResultByCardType(card.cardData.Type, success);

            healthBar.GainHealth(result.HealthDelta);
            timeBar.GainHealth(result.TimeDelta);

            var resultMessage = string.Format("{3} {4}\n{0}, {1}\n{2}",
                card.cardData.PlayString.Replace("%E", encounter.encounterData.Name),
                result.Text,
                string.Join(" ", new List<string>() {
                    result.HealthDelta != 0 ? string.Format("{0} Lives", result.HealthDelta.ToSignedString()) : "",
                    result.TimeDelta != 0 ? string.Format("{0} Time", result.TimeDelta.ToSignedString()) : "",
                }.Where(s => !string.IsNullOrEmpty(s))),
                success ? "Success!" : "Fail!",
                // Rolled 7 (+1)
                string.Format(
                    "Rolled {0} {1}",
                    value + card.cardData.Bonus,
                    card.cardData.Bonus > 0 ? string.Format("({0})", card.cardData.Bonus.ToSignedString()) : ""
                )
            );


            messageText.text = resultMessage;


            Debug.Log(string.Format(
                "Roll: {0} Result: {1} HealthDelta: {3} TurnDelta: {4}\nResult Message: {2}",
                value,
                success ? "Success" : "Fail",
                resultMessage,
                result.HealthDelta,
                result.TimeDelta
            ));

            NextEncounterButton.gameObject.SetActive(true);
        });

        

        _cards.Where(c => c != card).ToList().ForEach(c => {
            c.AnimateToThenDestroy(DiscardTarget.localPosition);
        });

        _cards = new List<Card> { card };

        card.AnimateTo(SelectedTarget.localPosition);
        card.AnimateToScale(Vector3.one * 1.3f);
        card.GetComponent<Button>().enabled = false;
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

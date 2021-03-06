using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public enum CardType
{
    Aggressive = 0,
    Neutral,
    Friendly,
    Stealth,
    Flee
}

[System.Serializable]
public class CardData
{
    public Sprite Image;
    public Sprite CanvasImage;
    public Sprite WhiskerImage;
    public string Action;
    public string Description;
    public int Bonus; // +/- to roll
    public CardType Type = CardType.Neutral;
    public string PlayString; // You attempt to ...
}


public static class IntExtensions
{
    public static string ToSignedString(this int value)
    {
        if (value > 0)
        {
            return "+" + value.ToString();
        }
        if (value == 0)
        {
            return "0";
        }
        return value.ToString();
    }
}


[RequireComponent(typeof(AnimateTo))]
[RequireComponent(typeof(AnimateToScale))]
[RequireComponent(typeof(AnimateToThenDestroy))]
public class Card : MonoBehaviour
{
    public TextMeshProUGUI ActionText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI ChanceText;
    public TextMeshProUGUI TypeText;
    public SpriteRenderer Image;
    public GameObject CanvasImage;
    public SpriteRenderer WhiskerImage;
    public TextMeshProUGUI TimeBonusText;
    public TextMeshProUGUI LifeBonusText;
    public GameObject DiceBonusImage;
    public TextMeshProUGUI DiceBonusText;

    public List<Sprite> DiceImages;

    public int ActionLineLength = 20;
    public int DescriptionLineLength = 40;

    public CardData cardData;

    private static System.Random rng = new System.Random();

    public void LoadCard(CardData card)
    {
        if (ActionText)
        {
            ActionText.text = ResolveTextSize(card.Action, ActionLineLength);
        }
        if (DescriptionText)
        {
            DescriptionText.text = ResolveTextSize(card.Description, DescriptionLineLength);
        }
        if (ChanceText)
        {
            ChanceText.text = card.Bonus.ToSignedString();
        }
        if (Image != null && card.Image != null)
        {
            Image.sprite = card.Image;
        }
        if (CanvasImage != null && card.CanvasImage != null)
        {
            CanvasImage.GetComponent<UnityEngine.UI.Image>().sprite = card.CanvasImage;
        }
        if (WhiskerImage != null && card.WhiskerImage != null)
        {
            WhiskerImage.sprite = card.WhiskerImage;
        }
        if (TypeText != null)
        {
            TypeText.text = card.Type.ToString();
        }
        // TODO made this show based on the encounter

        var encounter = Encounter.GetEncounter();
        if (TimeBonusText != null)
        {
            TimeBonusText.text = string.Format(
                "{0}/{1}",
                encounter.GetResultByCardType(card.Type, false).TimeDelta.ToSignedString(),
                encounter.GetResultByCardType(card.Type, true).TimeDelta.ToSignedString()
            );
        }
        if (LifeBonusText != null)
        {
            LifeBonusText.text = string.Format(
                "{0}/{1}",
                encounter.GetResultByCardType(card.Type, false).HealthDelta.ToSignedString(),
                encounter.GetResultByCardType(card.Type, true).HealthDelta.ToSignedString()
            );
        }

        if (DiceBonusImage != null)
        {
            if (cardData.Bonus > 0)
            {
                DiceBonusImage.GetComponent<UnityEngine.UI.Image>().sprite = DiceImages[cardData.Bonus - 1];
            }
            else
            {
                DiceBonusImage.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }
        if (DiceBonusText != null)
        {
            if (cardData.Bonus <= 0)
            {
                DiceBonusText.text = "";
            }
        }
    }

    public void AnimateTo(Vector3 targetPosition)
    {
        GetComponent<AnimateTo>().AnimateToPosition(targetPosition);
    }

    public void AnimateToThenDestroy(Vector3 targetPosition)
    {
        GetComponent<AnimateToThenDestroy>().AnimateToPosition(targetPosition);
    }

    public void AnimateToScale(Vector3 targetScale)
    {
        GetComponent<AnimateToScale>().Animate(targetScale);
    }

    private bool _hovering;
    public bool Hovering { get { return _hovering; } }

    public void OnHoverStart()
    {
        _hovering = true;
    }

    public void OnHoverEnd()
    {
        _hovering = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadCard(cardData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // https://answers.unity.com/questions/190800/wrapping-a-textmesh-text.html
    private string ResolveTextSize(string input, int lineLength)
    {
        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach(string s in words){
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if(temp.Length > lineLength){

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1,result.Length-1);
    }
}

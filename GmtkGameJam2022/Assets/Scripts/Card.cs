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
            return "";
        }
        return value.ToString();
    }
}


[RequireComponent(typeof(AnimateTo))]
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
    }

    public void AnimateTo(Vector3 targetPosition)
    {
        GetComponent<AnimateTo>().AnimateToPosition(targetPosition);
    }

    public void AnimateToThenDestroy(Vector3 targetPosition)
    {
        GetComponent<AnimateToThenDestroy>().AnimateToPosition(targetPosition);
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

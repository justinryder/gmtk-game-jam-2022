using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CardData
{
    public Sprite Image;
    public Sprite WhiskerImage;
    public string Action;
    public string Description;
    public int Chance;
}

public class Card : MonoBehaviour
{
    public TextMeshProUGUI ActionText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI ChanceText;
    public SpriteRenderer Image;
    public SpriteRenderer WhiskerImage;

    public int ActionLineLength = 20;
    public int DescriptionLineLength = 40;

    public CardData cardData;

    public void LoadCard(CardData card)
    {
        ActionText.text = ResolveTextSize(card.Action, ActionLineLength);
        DescriptionText.text = ResolveTextSize(card.Description, DescriptionLineLength);
        ChanceText.text = card.Chance.ToString() + "%";
        Image.sprite = card.Image;
        WhiskerImage.sprite = card.WhiskerImage;
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

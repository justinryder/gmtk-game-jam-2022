using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [System.Serializable]
    public class CardData
    {
        public string Image { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public int Chance { get; set; }
    }

    public TextMesh ActionText;
    public TextMesh DescriptionText;
    public TextMesh ChanceText;
    public SpriteRenderer Image;

    public int ActionLineLength = 20;
    public int DescriptionLineLength = 40;

    // public CardData cardData;

    public void LoadCard(CardData card)
    {
        ActionText.text = ResolveTextSize(card.Action, ActionLineLength);
        DescriptionText.text = ResolveTextSize(card.Description, DescriptionLineLength);
        ChanceText.text = card.Chance.ToString();
        
        var image = Resources.Load<Sprite>(card.Image);
        Image.sprite = image;
    }


    // Start is called before the first frame update
    void Start()
    {
        // LoadCard(cardData);
        LoadCard(
            new CardData
            {
                Image = "CardImage_Wave",
                Action = "Meow",
                Description = "Purrr... Maybe this human will pet us?",
                Chance = 20,
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

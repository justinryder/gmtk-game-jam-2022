using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public TextMesh TextMesh;
    public SpriteRenderer Image;
    public int LineLength = 40;

    public void LoadCard(string text, string imageName)
    {
        TextMesh.text = ResolveTextSize(text, LineLength);
        var image = Resources.Load<Sprite>(imageName);
        Image.sprite = image;
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadCard("Try to hold own back foot to clean it but foot reflexively kicks you in face, go into a rage and bite own foot, hard mew chase red laser dot. Oooo! dangly balls! jump swat swing flies so sweetly to the floor crash move on wash belly nap relentlessly pursues moth.", "CardInteraction_Stealth");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public TextMeshProUGUI TextMesh;
    public SpriteRenderer Image;


    public void LoadCard(string text, string imageName)
    {
        TextMesh.text = text;
        var image = Resources.Load<Sprite>(imageName);
        Image.sprite = image;
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadCard("ahjsdfhjkhasdhjkfhkjdasf", "CardInteraction_Stealth");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

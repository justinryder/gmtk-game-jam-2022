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
        LoadCard("Try to hold own back foot to clean it but foot reflexively kicks you in face, go into a rage and bite own foot, hard mew chase red laser dot. Oooo! dangly balls! jump swat swing flies so sweetly to the floor crash move on wash belly nap relentlessly pursues moth.", "CardInteraction_Stealth");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

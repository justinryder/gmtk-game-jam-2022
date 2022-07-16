using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPIPCONT : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;


    public void SetPip (Color color, Sprite image)
    {
        if (spriteRenderer)
        {
            spriteRenderer.material.color=color;

            spriteRenderer.sprite=image;
        }

        var _image = GetComponent<UnityEngine.UI.Image>();
        if (_image)
        {
            _image.sprite = image;
            _image.color = color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

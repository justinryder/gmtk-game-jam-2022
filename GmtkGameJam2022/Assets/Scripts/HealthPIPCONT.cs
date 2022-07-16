using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPIPCONT : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 

    public void SetPip (Color color, Sprite image)
    {
        spriteRenderer.material.color=color;

        spriteRenderer.sprite=image;
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

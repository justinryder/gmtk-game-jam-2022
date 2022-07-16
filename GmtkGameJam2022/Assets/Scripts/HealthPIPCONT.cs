using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPIPCONT : MonoBehaviour
{
    public void SetPip (bool on)
    {
        var renderer = GetComponent<Renderer>();

        if (on) 
        {
            renderer.material.color = Color.green;
        }
        else
        {
            renderer.material.color = Color.red;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPIPCONT : MonoBehaviour
{
    public Renderer renderer; 

    public void SetPip (Color color)
    {
        // var renderer = GetComponent<Renderer>();

        renderer.material.color=color;
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

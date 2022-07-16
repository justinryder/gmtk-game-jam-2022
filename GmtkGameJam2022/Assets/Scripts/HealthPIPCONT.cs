using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPIPCONT : MonoBehaviour
{
    public void SetPip (bool on)
    {
        if (on) 
        {
            gameObject.SetActive(true);

        } else 

        {
            gameObject.SetActive(false);
            
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

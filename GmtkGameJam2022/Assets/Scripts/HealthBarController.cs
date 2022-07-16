using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public List<HealthPIPCONT> HealthPips;

    void Sethealth(int health)
    {
        for(var i=0; i<HealthPips.Count; i++)
        {
            HealthPips[i].SetPip(i<health);


        }


    }

    // Start is called before the first frame update
    void Start()
    {
        Sethealth(5);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

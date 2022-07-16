using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turncontroller : MonoBehaviour
{
    public HealthBarController turnUI;

    private float timeSinceLastTurn = 0f; 

    public void endturn()
    {
        turnUI.LoseHealth(1);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastTurn += Time.deltaTime;
        if(timeSinceLastTurn >= 1)
        
        {
            timeSinceLastTurn = 0f;
            endturn();
        }
    }
}

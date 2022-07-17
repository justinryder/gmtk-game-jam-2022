using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gameController = GameController.GetGameController();
        if (!gameController)
        {
            Debug.Log("Can't start turn, no GameController tag found");
            return;
        }

        gameController.StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

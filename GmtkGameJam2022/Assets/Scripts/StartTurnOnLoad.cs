using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gameControllerObject = GameObject.FindWithTag("GameController");
        if (!gameControllerObject)
        {
            Debug.Log("Can't start turn, no GameController tag found");
        }

        var gameController = gameControllerObject.GetComponent<GameController>();
        gameController.StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

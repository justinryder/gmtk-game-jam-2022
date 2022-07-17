using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameControllerOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gameController = GameController.GetGameController();
        if (gameController)
        {
            Destroy(gameController.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

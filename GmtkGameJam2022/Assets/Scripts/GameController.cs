using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    void Awake()
    {
        var other = GameObject.FindWithTag("GameController");
        if (other != null && other != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        
    }
}

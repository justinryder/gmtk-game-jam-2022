using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopaftertime : MonoBehaviour
{

    public float duration = 10;

    private float start;

    private bool done; 

    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (done) return;

        if (Time.time>start+duration)
        
        {
            done=true; 

            gameObject.SetActive(false);

        }
    }
}

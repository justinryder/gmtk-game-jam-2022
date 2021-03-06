using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private bool debug = false;

    public float duration = 10;

    private float start;

    private bool done; 

    public string sceneName; 

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
                   
            SceneManager.LoadScene(sceneName);

        }

        if (debug)
        {
            if (Input.GetKeyUp(KeyCode.Slash))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
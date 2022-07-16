using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRED : MonoBehaviour
{
    public Color Startcolor;

    public float duration = 10;

    private float start;

    public Color Endcolor;

    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;



    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.color=Color.Lerp(Startcolor, Endcolor, (Time.time-start)/duration);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotationsPerSecond = 0.1f;
    public Vector3 RotationAxis = -Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RotationsPerSecond != 0)
        {
            transform.Rotate(RotationAxis * Time.deltaTime * RotationsPerSecond * 360);
        }
    }
}

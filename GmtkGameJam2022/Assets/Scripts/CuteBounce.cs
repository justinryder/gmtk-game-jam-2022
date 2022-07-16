using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuteBounce : MonoBehaviour
{
    public float BounceDuration = 0.2f;
    
    public float BounceDegrees = 2f;

    private bool _leftToRight = true;

    private int _direction = 0;

    private float _lastBounceTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastBounceTime + BounceDuration < Time.time)
        {
            _lastBounceTime = Time.time;

            _direction += _leftToRight ? 1 : -1;

            if (_direction > 1)
            {
                _direction = 1;
                _leftToRight = false;
            }

            if (_direction < -1)
            {
                _direction = -1;
                _leftToRight = true;
            }

            var degrees = _direction * BounceDegrees;

            transform.rotation = Quaternion.Euler(Vector3.forward * degrees);
        }
    }
}

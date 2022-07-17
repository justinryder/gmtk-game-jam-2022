using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateToScale : MonoBehaviour
{
    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private float _duration = 1;
    private float _start;
    private bool _done = true;
    private Action _onComplete;

    public void Animate(Vector3 targetScale, Action onComplete = null)
    {
        _initialScale = transform.localScale;
        _targetScale = targetScale;
        _start = Time.time;
        _done = false;
        _onComplete = onComplete;
    }

    void Update()
    {
        if (_done)
        {
            return;
        }

        var alpha = (Time.time - _start) / _duration;
        if (alpha >= 1)
        {
            _done = true;
            if (_onComplete != null)
            {
                _onComplete();
            }
        }

        transform.localScale = Vector3.Lerp(_initialScale, _targetScale, alpha);
    }
}

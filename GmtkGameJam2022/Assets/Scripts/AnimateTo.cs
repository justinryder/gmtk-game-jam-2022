using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTo : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private float _duration = 1;
    private float _start;
    private bool _done = false;
    private Action _onComplete;

    public void AnimateToPosition(Vector3 targetPosition, Action onComplete = null)
    {
        _initialPosition = transform.localPosition;
        _targetPosition = targetPosition;
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

        transform.localPosition = Vector3.Lerp(_initialPosition, _targetPosition, alpha);
    }
}

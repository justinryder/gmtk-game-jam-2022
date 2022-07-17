using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public List<Sprite> Faces;

    public float TimePerSide = 0.2f;
    public int TotalRolls = 8;

    private float _start;
    private int _rollCount;
    private int _value;
    private bool _rolling;
    private Action<int> _onComplete;

    private static System.Random rng = new System.Random();

    void SetImage(Sprite image)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = image;
    }

    public void Hide()
    {
        GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    public void Show()
    {
        GetComponent<UnityEngine.UI.Image>().enabled = true;
    }

    public void Roll(Action<int> onComplete = null)
    {
        _onComplete = onComplete;
        _start = Time.time;
        _rollCount = 0;
        _rolling = true;
        Show();
    }

    void Start()
    {
        Hide();
    }

    void Update()
    {
        if (!_rolling)
        {
            return;
        }

        if (Time.time > _start + TimePerSide)
        {
            _start = Time.time;

            _rollCount++;

            _value = rng.Next(0, 6);

            if (Faces.Count > _value)
            {
                SetImage(Faces[_value]);
                transform.Rotate(Vector3.forward * -45);
            }
            else
            {
                Debug.Log(string.Format("Can't set DiceRoller face for index {0}", _value));
            }

            if (_rollCount >= TotalRolls)
            {
                _rolling = false;
                if (_onComplete != null)
                {
                    _onComplete(_value);
                }
                // GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }
    }
}

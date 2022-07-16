using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
   public Color onColor = Color.green;

   public Color offColor = Color.red;
   
   private const bool DebugControls = true;

    public const int MaxHealth = 9;

    public List<HealthPIPCONT> HealthPips;

    private int _health;

    private void SetHealth(int health)
    {
        _health = health;

        for (var i = 0; i < HealthPips.Count; i++)
        {
            HealthPips[i].SetPip(i<health ? onColor : offColor);
        }
    }

    public void GainHealth(int deltaHealth)
    {
        var newHealth = Mathf.Min(_health + deltaHealth, MaxHealth);
        SetHealth(newHealth);
    }

    public void LoseHealth(int deltaHealth)
    {
        GainHealth(-deltaHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(9);
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugControls)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                GainHealth(1);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                LoseHealth(1);
            }
        }
    }
}

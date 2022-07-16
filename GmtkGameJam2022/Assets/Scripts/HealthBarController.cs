using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarController : MonoBehaviour
{
   public Sprite onImage;

   public Sprite offImage;

   public Color onColor = Color.green;

   public Color offColor = Color.white;

    public TextMeshProUGUI LabelText;
   
   private const bool DebugControls = true;

    public int MaxHealth = 9;

    public GameObject HealthPipPrefab;

    public Vector3 HealthPipSpacing = Vector3.right;

    public List<HealthPIPCONT> HealthPips;

    public string Label = "Lives:";

    private int _health;

    private void SetHealth(int health)
    {
        _health = health;

        for (var i = 0; i < HealthPips.Count; i++)
        {
            var on = i<health; 

            HealthPips[i].SetPip(on? onColor : offColor, on? onImage : offImage);
        }

        if (LabelText)
        {
            LabelText.text = string.Format("{0} {1}/{2}", Label, _health, MaxHealth);
        }
    }

    public void GainHealth(int deltaHealth)
    {
        var newHealth = Mathf.Clamp(_health + deltaHealth, 0 , MaxHealth);
        SetHealth(newHealth);
    }

    public void LoseHealth(int deltaHealth)
    {
        GainHealth(-deltaHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (var i = HealthPips.Count; i < MaxHealth; i++)
        {
            var pip = Instantiate(HealthPipPrefab, transform.position + HealthPipSpacing * i, transform.rotation);
            pip.transform.SetParent(transform);
            HealthPips.Add(pip.GetComponent<HealthPIPCONT>());
        }
        SetHealth(MaxHealth);
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

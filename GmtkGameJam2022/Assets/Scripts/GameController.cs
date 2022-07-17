using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private bool debug = false;

    public List<string> EncounterScenes;
    private int _encounterIndex;

    public string winScene;
    public string loseScene;

    public TextMeshProUGUI MessageText;

    public Deck deck;

    public Hand hand;

    public Button NextEncounterButton;
    public HealthBarController healthBar;
    public HealthBarController timeBar;

    public DiceRoller DiceRoller;

    public int DrawCount = 5;

    private static System.Random rng = new System.Random();

    public static GameController GetGameController()
    {
        var gameControllerObject = GameObject.FindWithTag("GameController");
        if (!gameControllerObject)
        {
            return null;
        }
        return gameControllerObject.GetComponent<GameController>();
    }

    void Awake()
    {
        var other = GameObject.FindWithTag("GameController");
        if (other != null && other != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (MessageText)
        {
            MessageText.text = "Your ears perk up to the sound of a can opening, but you are far from home.\nHurry back before your food goes cold!\nRoll 4 or higher after bonus to win an encounter.\nClick Next Encounter when you are ready to begin.";
        }
    }

    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyUp(KeyCode.Backslash))
            {
                LoadNextEncounter();
            }
        }
    }

    public void SetEncounterMessage()
    {
        var encounterGameObject = GameObject.FindWithTag("Encounter");
        
        if (!encounterGameObject)
        {
            Debug.Log("No Encounter tagged object in scene.");
            return;
        }
        
        var encounter = encounterGameObject.GetComponent<Encounter>();
        if (MessageText)
        {
            MessageText.text = encounter.encounterData.InitMessage;
        }
    }

    void StartEncounter()
    {
        SetEncounterMessage();

        Debug.Log("Starting encounter. Discard hand, draw new hand, disable next encounter button");

        if (hand)
        {
            hand.Discard();
        }

        NextEncounterButton.gameObject.SetActive(false);

        if (DiceRoller)
        {
            DiceRoller.Hide();
        }
    }

    public void StartTurn()
    {
        if (deck)
        {
            for (var i = 0; i < DrawCount; i++)
            {
                deck.Draw();
            }
        }
    }

    public void LoadNextEncounter()
    {
        if (EncounterScenes.Count == 0)
        {
            Debug.Log("No encounters");
            return;
        }

        if (healthBar.Health <= 0)
        {
            Debug.Log("Died");
            if (!string.IsNullOrEmpty(loseScene))
            {
                SceneManager.LoadScene(loseScene);

                NextEncounterButton.gameObject.SetActive(false);

                if (MessageText)
                {
                    MessageText.text = "You had 9 lives, but now you have none.\nRest up and try again!";
                }

                if (hand)
                {
                    hand.Discard();
                }

                if (DiceRoller)
                {
                    DiceRoller.Hide();
                }
            }
            return;
        }

        if (timeBar.Health <= 0)
        {
            Debug.Log("Out of Time");
            if (!string.IsNullOrEmpty(loseScene))
            {
                SceneManager.LoadScene(loseScene);
                
                NextEncounterButton.gameObject.SetActive(false);

                if (MessageText)
                {
                    MessageText.text = "You excitedly take a bite only to realize it's gone cold.\nTry to get home faster next time!";
                }

                if (hand)
                {
                    hand.Discard();
                }

                if (DiceRoller)
                {
                    DiceRoller.Hide();
                }
            }
            return;
        }

        if (_encounterIndex >= EncounterScenes.Count)
        {
            Debug.Log("Win!");
            if (!string.IsNullOrEmpty(winScene))
            {
                SceneManager.LoadScene(winScene);

                NextEncounterButton.gameObject.SetActive(false);

                if (MessageText)
                {
                    MessageText.text = "WINNER WINNER\nMEOW MIX MEOW MIX DID DELIVER!";
                }

                if (hand)
                {
                    hand.Discard();
                }

                if (DiceRoller)
                {
                    DiceRoller.Hide();
                }
            }
            return;
        }

        var scene = EncounterScenes[_encounterIndex];
        _encounterIndex++;

        SceneManager.LoadScene(scene);

        StartEncounter();
    }
}

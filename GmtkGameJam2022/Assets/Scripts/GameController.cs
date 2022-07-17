using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public List<string> EncounterScenes;

    public TextMeshProUGUI MessageText;

    public Deck deck;

    public Hand hand;

    public int DrawCount = 3;

    private static System.Random rng = new System.Random();

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
        StartEncounter();
    }

    void Update()
    {
        
    }

    void SetEncounterMessage()
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

        if (hand)
        {
            hand.Discard();
        }

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

        int index = rng.Next(EncounterScenes.Count);
        var scene = EncounterScenes[index];

        SceneManager.LoadScene(scene);

        StartEncounter();
    }
}

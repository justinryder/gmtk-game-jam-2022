using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<string> EncounterScenes;

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

    void Update()
    {
        
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
    }
}

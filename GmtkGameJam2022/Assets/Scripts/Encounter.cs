using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EncounterStats
{
    public int Aggressive;
    public int Neutral;
    public int Friendly;
    public int Stealth;
    public int Flee;
}

[System.Serializable]
public class EncounterResult
{
    public string Text;
    public int TimeDelta;
    public int HealthDelta;
}

[System.Serializable]
public class EncounterData
{
    public string Name; // The Mailman
    public string InitMessage; // The Mailman appraches you. They might step on your floofy tail! What do you do?

    public EncounterResult AggressiveSuccessResult;
    public EncounterResult AggressiveFailResult;

    public EncounterResult FriendlySuccessResult;
    public EncounterResult FriendlyFailResult;

    public EncounterResult StealthSuccessResult;
    public EncounterResult StealthFailResult;

    public EncounterResult NeutralSuccessResult;
    public EncounterResult NeutralFailResult;

    public EncounterResult FleeSuccessResult;
    public EncounterResult FleeFailResult;
}

public class Encounter : MonoBehaviour
{
    public EncounterData encounterData;

    public EncounterResult GetResultByCardType(CardType cardType, bool success)
    {
        switch(cardType)
        {
            case CardType.Aggressive:
                return success ? encounterData.AggressiveSuccessResult : encounterData.AggressiveFailResult;
            case CardType.Neutral:
                return success ? encounterData.NeutralSuccessResult : encounterData.NeutralFailResult;
            case CardType.Friendly:
                return success ? encounterData.FriendlySuccessResult : encounterData.FriendlyFailResult;
            case CardType.Stealth:
                return success ? encounterData.StealthSuccessResult : encounterData.StealthFailResult;
            case CardType.Flee:
                return success ? encounterData.FleeSuccessResult : encounterData.FleeFailResult;
            default:
                return new EncounterResult();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var gameControllerObject = GameObject.FindWithTag("GameController");
        if (!gameControllerObject)
        {
            Debug.Log("Can't set message, no GameController tag found");
        }

        var gameController = gameControllerObject.GetComponent<GameController>();
        gameController.SetEncounterMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

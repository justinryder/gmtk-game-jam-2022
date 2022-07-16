using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeterministicDiceRoller : MonoBehaviour
{
    public bool rollDice = false;
    public bool mode2D = false;
    public List<int> wantedRoll;
    public List<int> simulatedDiceRoll;
    public GameObject dicesGameObject;
    public List<GameObject> dicesList;
    public List<int> dicesSidesList;
    public bool returnToOrigin;
    public List<Vector3> originalPosition;
    public List<Vector3> originalRotation;
    public bool addForce = true;
    [Range(1, 100)]
    public int forceMultiplier = 10;
    public bool addTorque = true;
    [Range(1, 1000)]
    public int torqueMultiplier = 10;
    private Vector3 simulatedTorque;
    public int totalSimulationSteps;
    public float replaySteps = 0f;
    [Range(0, 1000)]
    public int replaySpeed = 200;
    private const int maxiterations = 2000;
    private RecordedInformation[] recordInformationdice1 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice2 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice3 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice4 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice5 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice6 = new RecordedInformation[maxiterations];
    private RecordedInformation[] recordInformationdice7 = new RecordedInformation[maxiterations];
    private bool simulationDone = false;
    private bool recordDiceRoll = false;
    private bool correctDiceRotation = false;
    private bool storeDices = false;
    [HideInInspector]
    public Rigidbody[] rbArray;
    private Vector3 defaultVector;
    [Range(0.01f, 0.10f)]
    public float simulationStep = 0.05f;
    [HideInInspector]
    public List<Rigidbody> unsetList;
    [HideInInspector]
    public List<Vector3> ridgidbodyoriginalPosition;
    [HideInInspector]
    public List<Vector3> ridgidbodyoriginalRotation;
    public bool edgelandingfix;
    [Range(0, 1000)]
    public int maxedgelandingfixiterations = 10;
    [Range(0, 10000)]
    public float edgelandingforce = 100f;
    public diceRollingBehaviour behaviour;
    public enum diceRollingBehaviour
    {
        SetKinematic = 0,
        CollideWithRigidbodies = 1,
        CollideWithRigidbodiesFreezeOnFinish = 2,
    }
    private float topSide;


    //Type for storing position and rotation of dices
    public struct RecordedInformation
    {
        public Vector3 recordedPosition;
        public Vector3 recordedRotation;

        public RecordedInformation(Vector3 recordedP, Vector3 recordedR)
        {
            recordedPosition = recordedP;
            recordedRotation = recordedR;
        }
    }

    //Function for recording postion of rotation of all rigidbodies.
    public void RecordPostitionAndRotationOfRidgidbodies()
    {
        //Debug.Log("RecordPostitionAndRotationOfRidgidbodies");

        //If array is already full, clear it.
        if (rbArray.Length > 0)
        {

            for (int i = 0; i < rbArray.Length; i++)
            {
                rbArray[i] = null;
            }
        }

        //Find all rigidbodies in scene
        rbArray = (Rigidbody[])GameObject.FindObjectsOfType(typeof(Rigidbody));

        //Loop through them and store position and rotation
        /*
        foreach (Rigidbody body in rbArray)
        {
            if (body.isKinematic == false)
            {
                if (body.transform.parent != dicesGameObject.transform)
                {
                    ridgidbodyoriginalPosition.Add(body.transform.position);
                    ridgidbodyoriginalRotation.Add(body.transform.eulerAngles);
                    unsetList.Add(body);
                }
            }
        }
        */

        for (int i = 0; i < rbArray.Length; i++)
        {
            if (rbArray[i].isKinematic == false)
            {
                if (rbArray[i].transform.parent != dicesGameObject.transform)
                {
                    ridgidbodyoriginalPosition.Add(rbArray[i].transform.position);
                    ridgidbodyoriginalRotation.Add(rbArray[i].transform.eulerAngles);
                    unsetList.Add(rbArray[i]);
                }
            }
        }
    }


    //Function for restoring all rigidbodies to their original position and rotation after simulation is done
    public void RestoreRigidbodiesPositionAndRotation()
    {

        //Debug.Log("RestoreRigidbodiesPositionAndRotation()");

        int rbcounter = 0;
        foreach (Rigidbody body in unsetList)
        {
            body.transform.position = ridgidbodyoriginalPosition[rbcounter];
            body.transform.eulerAngles = ridgidbodyoriginalRotation[rbcounter];
            rbcounter++;
        }
        //unsetList = new List<Rigidbody>();
        unsetList.Clear();
        ridgidbodyoriginalPosition.Clear();
        ridgidbodyoriginalRotation.Clear();
    }



    //Function for setting all Rigidbodies to Kinematic(This is done to prevent the simulation to collide with other rigidbodies).
    public void SetKinematic()
    {
        //Debug.Log("SetKinematic()");


        //Clear Array
        if (rbArray.Length > 0)
        {

            for (int i = 0; i < rbArray.Length; i++)
            {
                rbArray[i] = null;

            }
        }

        rbArray = (Rigidbody[])GameObject.FindObjectsOfType(typeof(Rigidbody));

        foreach (Rigidbody body in rbArray)
        {
            if (body.isKinematic == false)
            {
                if (body.transform.parent != dicesGameObject.transform)
                {
                    body.isKinematic = true;
                    unsetList.Add(body);
                }
            }
        }
    }

    //Function for setting all dices to Kinematic after the simulation is run, this freezes them on spot. Used when behaviour = Collide With Rigidbodies and Freeze on finish.
    public void SetDicesKinematic()
    {
        //Debug.Log("SetDicesKinematic()");

        foreach (GameObject dice in dicesList)
        {
            dice.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    //Function for unfreezing the dices. Used when behaviour = Collide With Rigidbodies and Freeze on finish.
    public void UnSetDicesKinematic()
    {
        //Debug.Log("UnsetDicesKinematic()");

        foreach (GameObject dice in dicesList)
        {
            dice.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //Function for setting all Rigidbodies to Kinematic again (used after simulation to resume real world physics for selected rigidbodies) 
    public void UnSetKinematic()
    {
        //Debug.Log("UnsetKinematic()");

        {
            foreach (Rigidbody body in unsetList)
            {
                body.isKinematic = false;

            }
            unsetList.Clear();
        }
    }

    //Function for storing all dices in a list.
    public void StoreDicesInList()
    {
        //Grab all dices under the dicesGameobject(DeterministicDices by default) and put the them in a list.


        for (int i = 0; i < dicesGameObject.transform.childCount; i++)
        {
            if (dicesGameObject.transform.GetChild(i).gameObject.activeSelf)
            {
                dicesList.Add(dicesGameObject.transform.GetChild(i).gameObject);

                //Add dice type( D6, D8, D20 to another list)

                if (dicesGameObject.transform.GetChild(i).GetComponent<DeterministicDice>() != null)
                {
                    dicesSidesList.Add((int)dicesGameObject.transform.GetChild(i).GetComponent<DeterministicDice>().diceType);
                }

                else
                {
                    dicesSidesList.Add((int)dicesGameObject.transform.GetChild(i).GetComponent<DeterministicDiceSkull>().diceType);
                }
            }
        }


        /*
        foreach (Transform child in dicesGameObject.transform)
        {
            if (child.gameObject.activeSelf)
            {
                dicesList.Add(child.gameObject);

                //Add dice type( D6, D8, D20 to another list)

                if (child.GetComponent<DeterministicDice>() != null)
                {
                    dicesSidesList.Add((int)child.GetComponent<DeterministicDice>().diceType);
                }

                else
                {
                    dicesSidesList.Add((int)child.GetComponent<DeterministicDiceSkull>().diceType);
                }
            }
        }
        */
    }

    //Function for populating the dices deterministic roll(Used by skulldice to see which side to light up).
    public void SetDeterministicDiceRoll()
    {

        int dicelistcount = dicesList.Count;
        //Grab all dices under the dicesGameobject(DeterministicDices by default) and put the them in a list.

        for (int diceindex = 0; diceindex < dicelistcount; diceindex++)

        {
            if (dicesList[diceindex].GetComponent<DeterministicDice>() != null)
            {
                dicesList[diceindex].GetComponent<DeterministicDice>().deterministicRoll = true;
                dicesList[diceindex].GetComponent<DeterministicDice>().diceRollDeterministic = wantedRoll[diceindex];
            }

            else
            {
                dicesList[diceindex].GetComponent<DeterministicDiceSkull>().deterministicRoll = true;
                dicesList[diceindex].GetComponent<DeterministicDiceSkull>().diceRollDeterministic = wantedRoll[diceindex];
            }

        }

    }

    //Function for storing all original position of the dices.
    public void StorePosition(GameObject dice)
    {
        //Store all original positions of the dices
        if (returnToOrigin)
        {
            if (dice.gameObject.activeSelf)
            {

                originalPosition.Add(dice.transform.position);
                originalRotation.Add(dice.transform.eulerAngles);
            }
        }
    }

    public void RestorePosition(GameObject dice, int diceindex)
    {
        //Only return to original if we set have that setting.
        if (returnToOrigin)
        {
            //Set all dices to their original positions and rotations
            dice.transform.position = originalPosition[diceindex];
            dice.transform.eulerAngles = originalRotation[diceindex];
        }
    }


    //Function for adding force to dices.
    public void UseForce(GameObject dice)
    {
        if (addForce)
        {
            //Add Force
            Vector3 force = Vector3.back * Random.Range(-400.0f, 400.0f) + Vector3.up * Random.Range(200.0f, 400.0f) + Vector3.right * Random.Range(-400.0f, 400.0f);
            dice.GetComponent<Rigidbody>().AddForce(force * forceMultiplier);
        }
    }

    //Function for adding torque to dices.
    public void UseTorque(GameObject dice)
    {
        if (addTorque)
        {
            //Add Torque
            dice.GetComponent<Rigidbody>().maxAngularVelocity = 100;
            simulatedTorque = new Vector3(UnityEngine.Random.Range(10, 180), UnityEngine.Random.Range(10, 180), UnityEngine.Random.Range(10, 180));
            dice.GetComponent<Rigidbody>().AddTorque(simulatedTorque * torqueMultiplier);
        }
    }

    //This is the function that rolls the dices Normally(Non-Deterministically).
    public void NormalRoll()
    {
        int diceindex = 0;
        foreach (GameObject dice in dicesList)
        {

            //Reset rotation of Mesh
            //dice.transform.GetChild(6).localEulerAngles = new Vector3(0, 0, 0);

            //Restore Position of dices
            RestorePosition(dice, diceindex);

            //Activate Gravity.
            dice.GetComponent<Rigidbody>().useGravity = true;

            UseTorque(dice);
            UseForce(dice);

            diceindex++;

        }

        //Debug.Log("Rolling Dice Normally");
    }

    // Start is called before the first frame update
    void Start()
    {

        //Store all dices in a list
        StoreDicesInList();

        //Store all dice positions
        foreach (GameObject dice in dicesList)
        {
            StorePosition(dice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Main Simulation code - Grab All Dices -> Simulate -> Record -> Stop.
        if (rollDice)
        {

            //Make sure wantedroll list is populated. Otherwise add number 1 as the roll we want to make.
            if (wantedRoll.Count < dicesList.Count)
            {
                for (int index = wantedRoll.Count; index < dicesList.Count; index++)
                    wantedRoll.Add(1);
            }

            //Populate all dices with what they should roll. Done mainly for the Skulldice so it will now which side to light up.
            SetDeterministicDiceRoll();

            //Find all other rigidbodies and set them to kinematic(This solves the problem when the dices collides with rigidbodies during the simulation).
            if ((int)behaviour == 0)
            {
                SetKinematic();
                UnSetDicesKinematic();
                //   Debug.Log("Rolling Dice Deterministically. Behaviour: Set Kinematic");
            }

            //Find all other rigidbodies and save their position, then run the simulation and afterwards restore the position of all rigidbodies;

            if ((int)behaviour == 1)
            {
                RecordPostitionAndRotationOfRidgidbodies();
                UnSetDicesKinematic();
                //   Debug.Log("Rolling Dice Deterministically. Behaviour: Collide With Rigidbodies");
            }


            //Find all other rigidbodies and save their position, then run the simulation and afterwards restore the position of all rigidbodies, when finished freeze all dices on spot;

            if ((int)behaviour == 2)
            {
                RecordPostitionAndRotationOfRidgidbodies();
                UnSetDicesKinematic();
                //    Debug.Log("Rolling Dice Deterministically. Behaviour: Collide With Rigidbodies Freeze on Finish");
            }


            //Check if we have changed the number of dices between simulations.
            if (!storeDices)
            {

                dicesList.Clear();
                dicesSidesList.Clear();

                //Store all dices in a list.
                StoreDicesInList();

                storeDices = true;
            }

            //Turn off Physics autosimulation
            Physics.autoSimulation = false;

            //Reset Variables
            replaySteps = 0;

            //Restore positions and add force and torque(if the settings = true)
            int diceCount = 0;
            foreach (GameObject dice in dicesList)
            {
                //Restore Position of dices.
                RestorePosition(dice, diceCount);

                //Activate Gravity.
                dice.GetComponent<Rigidbody>().useGravity = true;

                UseTorque(dice);
                UseForce(dice);

                //Next Dice
                diceCount++;
            }

            //Used for edgelandingfixing
            int edgelandingfixiterations = 0;
            bool applydownforce = false;

            //Simulation code. Simulate max 2000 steps. Move 0.01 steps at a time. 
            for (int index = 0; index < maxiterations; index++)
            {
                totalSimulationSteps = index;
                Physics.Simulate(simulationStep);


                //Record the information of each dice into an Array with the custom Type RecordedInformation(Defined at the top).
                int diceCountSimulated = 0;
                foreach (GameObject dice in dicesList)
                {

                    switch (diceCountSimulated)
                    {
                        case 6:
                            recordInformationdice7[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        case 5:
                            recordInformationdice6[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        case 4:
                            recordInformationdice5[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        case 3:
                            recordInformationdice4[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        case 2:
                            recordInformationdice3[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        case 1:
                            recordInformationdice2[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                        default:
                            recordInformationdice1[totalSimulationSteps] = new RecordedInformation(dice.transform.position, dice.transform.eulerAngles);
                            break;
                    }
                    diceCountSimulated++;
                }

                //Check if any dice is still rolling

                int diceCountResting = 0;
                bool dicesSleeping = true;
                foreach (GameObject dice in dicesList)
                {
                    if (!dice.GetComponent<Rigidbody>().IsSleeping())
                    {

                        dicesSleeping = false;
                        diceCountResting++;
                    }
                }

                //If all dices are resting, stop simulation.
                if (dicesSleeping)
                {

                    Vector3 edgelandingvector = Vector3.back * 2 + Vector3.up * -10 + Vector3.right * 2;

                    //If we have enabled edgelanding fixing, apply a downforce to all dices and run the simulation X amount of times more.
                    if (edgelandingfix)

                    {
                        //Apply downforce to all dices that are resting
                        if (!applydownforce)
                        {
                            foreach (GameObject dice in dicesList)
                            {


                                dice.GetComponent<Rigidbody>().AddForce(Vector3.down * 10 * edgelandingforce);

                            }

                            applydownforce = true;
                        }

                        //Run the simulation X amounts of times more 
                        if (edgelandingfixiterations < maxedgelandingfixiterations)

                        {

                            edgelandingfixiterations++;

                        }

                        //if we have run the simulation x amounts of times more, then stop it.
                        else
                        {
                            break;
                        }

                    }

                    //if we dont have edgelandingfix enabled then stop the simulation
                    else
                    {
                        break;
                    }
                }
            }

            //Done with the Simulation
            simulationDone = true;
            rollDice = false;
            recordDiceRoll = false;
            correctDiceRotation = false;

        }


        //Main Replay code - Grab all Dices -> Replay -> Stop.

        if (simulationDone)
        {

            //Save what each dice rolled during the simulation.
            if (!recordDiceRoll)
            {
                //Clear old results
                simulatedDiceRoll = new List<int>();

                int dicetopcheckcount = 0;
                //For each dice, loop through the sides and find which one has the highest Y position.
                foreach (GameObject dice in dicesList)
                {

                    int finalDiceRoll = 0;

                    //Check if we are using 2D mode, if not then use the Y axis for calculations
                    if (mode2D == false)
                    {
                        topSide = -50000;
                    }

                    //If we are using 2D mode then use the Z axis
                    else
                    {
                        topSide = 50000;
                    }

                    int diceRoll = 0;


                    //Loop through the sides(based on D6,D8,D20) and check whichever is highest.
                    for (int childindex = 0; childindex < dicesSidesList[dicetopcheckcount]; childindex++)
                    {
                        var getDiceSide = dice.transform.GetChild(childindex);

                        //Check if we are using 2D mode, if not then use the Y axis for calculations
                        if (mode2D == false)
                        {
                            //If the side is higher then the previous checked side, update diceRoll.
                            if (getDiceSide.position.y > topSide)
                            {
                                topSide = getDiceSide.position.y;
                                diceRoll = childindex + 1;
                                //Debug.Log("Roll:" + diceRoll);
                            }
                        }

                        //If we are using 2D mode then use the Z axis. Keep in mind gravity should be set to Z = 9.81 in the project settings.
                        else
                        {

                            //If the side is higher then the previous checked side, update diceRoll.
                            if (getDiceSide.position.z < topSide)
                            {
                                topSide = getDiceSide.position.z;
                                diceRoll = childindex + 1;
                                //Debug.Log("Roll:" + diceRoll);
                            }
                        }

                        finalDiceRoll = diceRoll;

                    }
                    //Store in List
                    simulatedDiceRoll.Add(finalDiceRoll);

                    dicetopcheckcount++;

                }
                //Only record the dice roll once.
                recordDiceRoll = true;


                //Restore all rigidbodies to (Kinematic = false) if behaviour is set to SetKinematic.
                if ((int)behaviour == 0)
                {
                    UnSetKinematic();
                }

                //Restore position and rotation of all rigidbodies if behaviour is set to CollideWithRigidbodies.
                if ((int)behaviour == 1)
                {
                    RestoreRigidbodiesPositionAndRotation();
                }

                //Restore position and rotation of all rigidbodies if behaviour is set to CollideWithRigidbodies.
                if ((int)behaviour == 2)
                {
                    RestoreRigidbodiesPositionAndRotation();
                }

            }



            //Start Replaying
            replaySteps = (replaySteps + (Time.deltaTime * replaySpeed));
            //If we have not finished replaying it back, continue to replay.
            if (replaySteps < totalSimulationSteps)
            {
                int dicePositionCount = 0;

                foreach (GameObject dice in dicesList)
                {

                    switch (dicePositionCount)
                    {
                        case 6:
                            dice.transform.position = recordInformationdice7[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice7[(int)replaySteps].recordedRotation;
                            break;
                        case 5:
                            dice.transform.position = recordInformationdice6[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice6[(int)replaySteps].recordedRotation;
                            break;
                        case 4:
                            dice.transform.position = recordInformationdice5[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice5[(int)replaySteps].recordedRotation;
                            break;
                        case 3:
                            dice.transform.position = recordInformationdice4[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice4[(int)replaySteps].recordedRotation;
                            break;

                        case 2:
                            dice.transform.position = recordInformationdice3[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice3[(int)replaySteps].recordedRotation;
                            break;

                        case 1:
                            dice.transform.position = recordInformationdice2[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice2[(int)replaySteps].recordedRotation;
                            break;

                        case 0:
                            dice.transform.position = recordInformationdice1[(int)replaySteps].recordedPosition;
                            dice.transform.eulerAngles = recordInformationdice1[(int)replaySteps].recordedRotation;
                            break;

                    }
                    //Debug.Log("We are at replayStep: " + replaySteps + "/" +totalSimulationSteps+" and Position" + dice.transform.position);
                    dicePositionCount++;
                }


                //Turn Physics Back on
                Physics.autoSimulation = true;

                //After the replay has started(This hides that we are rotating the dice :) ) turn the dice to the correct rotation so that it lands on the number we want.
                if (!correctDiceRotation)
                {

                    int dicelistcount = 0;
                    Vector3 wantedRollDirection;
                    Vector3 simulatedRollDirection;
                    foreach (GameObject dice in dicesList)
                    {
                        if (simulatedDiceRoll[dicelistcount] != 0)
                        {


                            //Reset Mesh to original position.
                            dice.transform.GetChild(dicesSidesList[dicelistcount]).localEulerAngles = new Vector3(0, 0, 0);
                            defaultVector = new Vector3(0, 0, 0);

                            //Get the directions for the roll we want.
                            wantedRollDirection = dice.transform.GetChild((wantedRoll[dicelistcount]) - 1).localPosition - defaultVector;
                            //Debug.DrawLine(dice.transform.GetChild((wantedRoll[dicelistcount]) - 1).localPosition, defaultVector, Color.green, 200);

                            //Get the directions for the roll we simulated.
                            simulatedRollDirection = dice.transform.GetChild((simulatedDiceRoll[dicelistcount]) - 1).localPosition - defaultVector;
                            //Debug.DrawLine(dice.transform.GetChild((simulatedDiceRoll[dicelistcount]) - 1).localPosition, defaultVector, Color.red, 200);

                            //Rotate the mesh to the correct position
                            dice.transform.GetChild(dicesSidesList[dicelistcount]).localRotation *= Quaternion.FromToRotation(wantedRollDirection, simulatedRollDirection);
                            //Debug.Log("Rotating: " + wantedRollDirection + " " + simulatedRollDirection);

                            dicelistcount++;

                        }

                        //If the simulation dice returned 0.
                        else
                        {
                            Debug.LogWarning("WARNING: Dice " + dice + " did not finish the simulation! Most likely it fell of the board/disappeared. Did you add to much force to it?");
                        }
                    }

                }
                correctDiceRotation = true;
            }


            else
            {

                //If behaviour is set to Collide WIth Rigidbodies and Freeze, freeze the dices.
                if ((int)behaviour == 2)
                {
                    SetDicesKinematic();
                }

                //Reset simulation status so we are ready for another run(if any).
                simulationDone = false;
                storeDices = false;
                correctDiceRotation = false;


            }

        }
    }

}





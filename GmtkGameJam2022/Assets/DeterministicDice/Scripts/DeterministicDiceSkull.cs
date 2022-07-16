using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeterministicDiceSkull : MonoBehaviour
{
    // Start is called before the first frame update

    public bool diceSleeping;
    public bool diceReset;
    public bool deterministicRoll;
    public int diceRoll;
    public int diceRollDeterministic;
    private float topSide;
    public Color skullDiceColor;
    private Material topDiceMaterial;
    [Range(0.0f, 10.0f)]
    public float minIntensity = 0.01f;
    [Range(0.0f, 10.0f)]
    public float maxIntensity = 0.01f;
    [Range(0.0f, 10.0f)]
    public float pulsateSpeed = 0.01f;
    [Range(0.0f, 10.0f)]
    public float pulsateMaxDistance = 0.01f;
    public Color originalColor;
    private Material[] skulldicematerials;
    public diceTypeList diceType;

    public enum diceTypeList
    {

        D6 = 6,
        D8 = 8,
        D20 = 20,
    }

    void Start()
    {


        //Here you can add Torque and Force do a dice manually if you would like.


       

        //Grab all materials and enable Emission keyword so we can change it programatically.
        Material[] skulldicematerials = gameObject.transform.GetChild(6).GetComponent<Renderer>().materials;

        for (int index = 0; index < 7; index++)
        {
            skulldicematerials[index].EnableKeyword("_EMISSION");
            
        }

        topDiceMaterial = skulldicematerials[1];


    }

    // Update is called once per frame
    void Update()
    {


        //Only execute when the dice has stopped rolling
        if (gameObject.GetComponent<Rigidbody>().IsSleeping())

        {
            //Only execute until we got a result
            if (!diceSleeping)
            {
                diceReset = false;

                if (!deterministicRoll)

                {
                    //Reset the variable that contains the topmost side of the dice
                    topSide = -5000000;


                    //Loop through the 6 sides of the dice and find which one has the highest transform.Y
                    for (int index = 0; index < 6; index++)
                    {
                        var getChild = gameObject.transform.GetChild(index);

                        if (getChild.position.y > topSide)
                        {
                            topSide = getChild.position.y;
                            diceRoll = index + 1;

                            //Debug.Log("Roll:" + diceRoll);

                        }


                    }

                    Material[] mat = gameObject.transform.GetChild(6).GetComponent<Renderer>().materials;



                    switch (diceRoll)
                    {


                        case 6:

                            topDiceMaterial = mat[3];
                            break;
                        case 5:

                            topDiceMaterial = mat[2];
                            break;
                        case 4:

                            topDiceMaterial = mat[4];
                            break;

                        case 3:

                            topDiceMaterial = mat[6];
                            break;

                        case 2:

                            topDiceMaterial = mat[5];
                            break;

                        case 1:

                            topDiceMaterial = mat[1];
                            break;

                    }


                }

                else

                {
                    Material[] mat = gameObject.transform.GetChild(6).GetComponent<Renderer>().materials;

                    switch (diceRollDeterministic)
                    {


                        case 6:

                            topDiceMaterial = mat[3];
                            break;
                        case 5:

                            topDiceMaterial = mat[2];
                            break;
                        case 4:

                            topDiceMaterial = mat[4];
                            break;

                        case 3:

                            topDiceMaterial = mat[6];
                            break;

                        case 2:

                            topDiceMaterial = mat[5];
                            break;

                        case 1:

                            topDiceMaterial = mat[1];
                            break;

                    }



                }

            }
            diceSleeping = true;

            //Sometimes dices land exactly on their edge, this makes sure that they dont by applying a small downforce when they have stopped.
            //gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down);

            //Change the intensity of the glow when landing and start pulsating.
            float emission = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * pulsateSpeed, pulsateMaxDistance));
            Color baseColor = skullDiceColor;
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            topDiceMaterial.SetColor("_EmissionColor", finalColor);


        }

        else
        {
            diceSleeping = false;


            if (!diceReset) { 

                Material[] skulldicematerials = gameObject.transform.GetChild(6).GetComponent<Renderer>().materials;
                //Reset colors
                for (int index = 1; index < 7; index++)
                {
                    skulldicematerials[index].SetColor("_EmissionColor", originalColor); ;

                }


                diceReset = true;
            }

        }


    }
}

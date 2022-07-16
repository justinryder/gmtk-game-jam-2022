using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Button1to6 : MonoBehaviour
{

    public GameObject diceRoll;
    public GameObject rigidbodies;
    public GameObject deterministicdices;
    public DeterministicDiceRoller diceRollScript;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button rollDiceDeterministic;
    public Button rollDiceNormal;
    public Button pokerDices;
    public Button rpgDices;
    public Toggle addForce;
    public Toggle addTorque;
    public Toggle returnToOrigin;
    public Toggle addRigidbodies;
    public Text wantedRoll;
    public Text diceRollText;
    public Slider forceSlider;
    public Slider replaySpeedSlider;
    public Slider torqueSlider;
    public Dropdown textureDropdown;
    public Dropdown behaviourDropdown;
    public Material diceMaterial;
    public Material diceMaterialD8;
    public Material diceMaterialD20;
    public Texture normalNumbers;
    public Texture normalDots;
    public Texture blackGreenDots;
    public Texture blackWhiteDots;
    public Texture blackWhiteNumbers;
    public Texture blueWhiteNumbers;
    public Texture redWhiteNumbers;
    public Texture stripesWhiteNumbers;
    public Texture whiteBlackDots;
    public Texture whiteBlackNumbers;
    public Texture whiteRedDots;
    public Texture yellowWhiteNumbers;
    public Texture D6HandpaintedWhite;
    public Texture D8HandpaintedWhite;
    public Texture D20HandpaintedWhite;
    public Texture D6HandpaintedBlue;
    public Texture D8HandpaintedBlue;
    public Texture D20HandpaintedBlue;
    public Texture D6HandpaintedTeal;
    public Texture D8HandpaintedTeal;
    public Texture D20HandpaintedTeal;
    public Texture D6HandpaintedOrange;
    public Texture D8HandpaintedOrange;
    public Texture D20HandpaintedOrange;
    public Texture D6HandpaintedPurple;
    public Texture D8HandpaintedPurple;
    public Texture D20HandpaintedPurple;
    public Texture D6HandpaintedGreen;
    public Texture D8HandpaintedGreen;
    public Texture D20HandpaintedGreen;
    public Texture D6HandpaintedRed;
    public Texture D8HandpaintedRed;
    public Texture D20HandpaintedRed;
    public Texture D6HandpaintedYellow;
    public Texture D8HandpaintedYellow;
    public Texture D20HandpaintedYellow;
    public Texture D6HandpaintedFuturistic;
    public Texture D8HandpaintedFuturistic;
    public Texture D20HandpaintedFuturistic;


    public float forceFloat;
    public int wantedRoll1;

    // Start is called before the first frame update
    void Start()
    {

        forceSlider.onValueChanged.AddListener((forcefloat) => ForceSlider(forcefloat));
        replaySpeedSlider.onValueChanged.AddListener((replayfloat) => ReplaySpeed(replayfloat));
        torqueSlider.onValueChanged.AddListener((torquefloat) => TorqueSlider(torquefloat));

        textureDropdown.onValueChanged.AddListener((textureInt) => ChangeTexture(textureInt));

        behaviourDropdown.onValueChanged.AddListener((behaviourInt) => ChangeBehaviour(behaviourInt));

        addForce = addForce.GetComponent<Toggle>();
        addForce.onValueChanged.AddListener((bool on) => Force());

        addTorque = addTorque.GetComponent<Toggle>();
        addTorque.onValueChanged.AddListener((bool on) => Torque());

        returnToOrigin = returnToOrigin.GetComponent<Toggle>();
        returnToOrigin.onValueChanged.AddListener((bool on) => Return());

        addRigidbodies = addRigidbodies.GetComponent<Toggle>();
        addRigidbodies.onValueChanged.AddListener((bool on) => AddRigidbodies());

        Button btn1 = button1.GetComponent<Button>();
        btn1.onClick.AddListener(Button1Click);

        Button btn2 = button2.GetComponent<Button>();
        btn2.onClick.AddListener(Button2Click);

        Button btn3 = button3.GetComponent<Button>();
        btn3.onClick.AddListener(Button3Click);

        Button btn4 = button4.GetComponent<Button>();
        btn4.onClick.AddListener(Button4Click);

        Button btn5 = button5.GetComponent<Button>();
        btn5.onClick.AddListener(Button5Click);

        Button btn6 = button6.GetComponent<Button>();
        btn6.onClick.AddListener(Button6Click);

        Button btnrollDiceDeterministic = rollDiceDeterministic.GetComponent<Button>();
        btnrollDiceDeterministic.onClick.AddListener(rollDiceDeterministicClick);

        Button btnrollDiceNormal = rollDiceNormal.GetComponent<Button>();
        btnrollDiceNormal.onClick.AddListener(rollDiceNormalClick);

        Button btnpokerDices = pokerDices.GetComponent<Button>();
        btnpokerDices.onClick.AddListener(pokerDicesClick);

        Button btnrpgDices = rpgDices.GetComponent<Button>();
        btnrpgDices.onClick.AddListener(rpgDicesClick);



    }

    void ChangeTexture(int textureInt)
    {

        diceMaterial.EnableKeyword("_NORMALMAP");

        switch (textureInt)

        {
            case 18:

                diceMaterial.SetTexture("_MainTex", D6HandpaintedFuturistic);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 17:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedYellow);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedYellow);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedYellow);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 16:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedRed);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedRed);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedRed);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 15:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedGreen);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedGreen);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedGreen);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 14:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedPurple);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedPurple);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedPurple);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 13:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedOrange);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedOrange);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedOrange);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;


            case 12:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedTeal);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedTeal);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedTeal);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 11:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedBlue);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedBlue);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedBlue);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 10:
                diceMaterialD20.SetTexture("_MainTex", D20HandpaintedWhite);
                diceMaterialD20.SetTexture("_BumpMap", null);
                diceMaterialD8.SetTexture("_MainTex", D8HandpaintedWhite);
                diceMaterialD8.SetTexture("_BumpMap", null);
                diceMaterial.SetTexture("_MainTex", D6HandpaintedWhite);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 9:
                diceMaterial.SetTexture("_MainTex", yellowWhiteNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;

            case 8:
                diceMaterial.SetTexture("_MainTex", whiteRedDots);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;
            case 7:
                diceMaterial.SetTexture("_MainTex", whiteBlackNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;
            case 6:
                diceMaterial.SetTexture("_MainTex", whiteBlackDots);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;
            case 5:
                diceMaterial.SetTexture("_MainTex", stripesWhiteNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;
            case 4:
                diceMaterial.SetTexture("_MainTex", redWhiteNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;
            case 3:
                diceMaterial.SetTexture("_MainTex", blueWhiteNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;
            case 2:
                diceMaterial.SetTexture("_MainTex", blackWhiteNumbers);
                diceMaterial.SetTexture("_BumpMap", normalNumbers);
                break;

            case 1:
                diceMaterial.SetTexture("_MainTex", blackWhiteDots);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;

            case 0:
                diceMaterial.SetTexture("_MainTex", blackGreenDots);
                diceMaterial.SetTexture("_BumpMap", normalDots);
                break;
        }
    }

    void ChangeBehaviour(int behaviourInt)
    {


        switch (behaviourInt)
        {

            case 2:
                diceRollScript.behaviour = DeterministicDiceRoller.diceRollingBehaviour.CollideWithRigidbodiesFreezeOnFinish;
;                break;

            case 1:
                diceRollScript.behaviour = DeterministicDiceRoller.diceRollingBehaviour.CollideWithRigidbodies;
                break;

            case 0:
                diceRollScript.behaviour = DeterministicDiceRoller.diceRollingBehaviour.SetKinematic;
                break;
        }
    }

    void ForceSlider(float forcefloat)
    {
        diceRollScript.forceMultiplier = (int)forcefloat;

    }

    void TorqueSlider(float torquefloat)
    {
        diceRollScript.torqueMultiplier = (int)torquefloat;

    }

    void ReplaySpeed(float replaySpeed)
    {
        diceRollScript.replaySpeed = (int)replaySpeed;

    }

    void Force()
    {
        diceRollScript.addForce = !diceRollScript.addForce;
        
    }

    void Torque()
    {
        diceRollScript.addTorque = !diceRollScript.addTorque;

    }

    void AddRigidbodies()
    {

        //Debug.Log("running code");

        if (rigidbodies.activeSelf == true)
        {

            rigidbodies.SetActive(false);

            //Debug.Log("setting rigidbvodies to false");
        }

        else 

        {

            rigidbodies.SetActive(true);

            //Debug.Log("setting rigidbvodies to active");
        }

    }


    void Return()
    {
        diceRollScript.returnToOrigin= !diceRollScript.returnToOrigin;

    }

    //Function for populating the dices deterministic roll(Used by skulldice to see which side to light up).
    public void UnSetDeterministicDiceRoll()
    {

        int dicelistcount = diceRollScript.dicesList.Count;
        //Grab all dices under the dicesGameobject(DeterministicDices by default) and put the them in a list.

        for (int diceindex = 0; diceindex < dicelistcount; diceindex++)

        {
            if (diceRollScript.dicesList[diceindex].GetComponent<DeterministicDice>() != null)
            {
                diceRollScript.dicesList[diceindex].GetComponent<DeterministicDice>().deterministicRoll = false;

            }

            else
            {
                diceRollScript.dicesList[diceindex].GetComponent<DeterministicDiceSkull>().deterministicRoll = false;

            }

        }

    }


    void Button1Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();
        }
        diceRollScript.wantedRoll.Add(1);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }

    void Button2Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();

        }
        diceRollScript.wantedRoll.Add(2);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;

    }

    void Button3Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();

        }
        diceRollScript.wantedRoll.Add(3);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }

    void Button4Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();

        }
        diceRollScript.wantedRoll.Add(4);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }

    void Button5Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();

        }
        diceRollScript.wantedRoll.Add(5);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }

    void Button6Click()
    {
        int wantedRollCount = diceRollScript.wantedRoll.Count;
        int dicecount = diceRollScript.dicesList.Count;

        if (wantedRollCount >= dicecount)
        {
            diceRollScript.wantedRoll = new List<int>();

        }
        diceRollScript.wantedRoll.Add(6);

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }


    //Deterministic Roll
    private void rollDiceDeterministicClick()
    {
        //This is what rolls the dices
        diceRollScript.rollDice = true;

        //Disable Normal roll text and enable Deterministic roll text.
        diceRollText.enabled = false;
        wantedRoll.enabled = true;
    }
    
    //Normal Roll
    private void rollDiceNormalClick()
    {
        UnSetDeterministicDiceRoll();
        diceRollScript.NormalRoll();

        //Disable Deterministic roll text and enable Normal roll text.
        diceRollText.enabled = true;
        wantedRoll.enabled = false;

    }

    //Switch dices to Pokerdices
    private void pokerDicesClick()
    {

        //switch material to red/white
        diceMaterial.SetTexture("_MainTex", redWhiteNumbers);
        diceMaterial.SetTexture("_BumpMap", normalNumbers);

        //activate all Poker dices

        for (int i = 7; i < 14; i++)
        {
            deterministicdices.transform.GetChild(i).gameObject.SetActive(true);
                
        }

        //disable all RPG dices

        for (int i = 0; i < 7; i++)
        {
            deterministicdices.transform.GetChild(i).gameObject.SetActive(false);

        }
    }

    //Switch dices to RPGDices
    private void rpgDicesClick()
    {
        //switch material to teal
        diceMaterial.SetTexture("_MainTex", D6HandpaintedTeal);
        diceMaterial.SetTexture("_BumpMap", normalDots);

        //de-activate all Poker dices
        for (int i = 7; i < 14; i++)
        {
            deterministicdices.transform.GetChild(i).gameObject.SetActive(false);

        }

        //Activate all RPG dices

        for (int i = 0; i < 7; i++)
        {
            deterministicdices.transform.GetChild(i).gameObject.SetActive(true);

        }
    }


    // Update is called once per frame
    void Update()
    {

        //Update Deterministic Dice Roll Text
            if (diceRollScript.wantedRoll.Count != 0)
            {

            wantedRoll.text = "";
            for (int index = 0; index < diceRollScript.wantedRoll.Count; index++)
                {
                    wantedRoll1 = diceRollScript.wantedRoll[index];
                    wantedRoll.text = (wantedRoll.text + "" + wantedRoll1.ToString());
                }
            }

        //Update Normal Roll Text
        if (diceRollScript.dicesList.Count != 0)
        {
            diceRollText.text = "";
            foreach (GameObject dice in diceRollScript.dicesList)
            {
               int diceroll = dice.GetComponent<DeterministicDice>().diceRoll;
                diceRollText.text = (diceRollText.text + " " + diceroll.ToString());
            }
        }
    }
}

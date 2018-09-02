/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum suitTypeSelect { Chars, Dots, Bams, Winds, Dragon , No }

[System.Serializable]
public struct TheButton
{
    public Button myButton;
    public bool activated;
    public bool pressed;
    public int id;
}

public class UiMaster : MonoBehaviour {

    public GameMaster _GameMaster;

    public TheButton[] numbers = new TheButton[10];
    public TheButton[] windNumbers = new TheButton[8];
    public TheButton enter;
    public TheButton back;
    public TheButton ok;
    public TheButton dots;
    public TheButton chars;
    public TheButton bams;
    public TheButton winds;
    public GameObject WindsButtons;
    public GameObject NumbersButtons;


    public suitTypeSelect currentType = suitTypeSelect.No;
    // public int[] entering = new int[17];  // ?

    public string displaying;
    public Text showText;

    void Awake()
    {
        _GameMaster = GameMaster.instance;

        TheButton[] temp = new TheButton[] { enter, back, ok, dots, chars, bams, winds };
        foreach (TheButton element in temp)
        {
            element.myButton.onClick.AddListener(() => buttonClick(element));
        }

        for (int i = 1; i <10; i++)
        {
            setButton(numbers[i]);
        }

        for (int i = 1; i < 8; i++)
        {
            setButton(windNumbers[i]);
        }
    }

    // Use this for initialization
    void Start () {
        currentType = suitTypeSelect.No;
        displaying = "Chooes a suits (筒/索/萬/字 ?)";
        showText.text = displaying;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int tileID (int suitID , suitTypeSelect type)
    {
        int _temp = 0;

        for (int i = 1; i < _GameMaster.tile144.Length; i++)
        {
            if ((int)type <= 4)
            {
                if (suitID > 4 && (int)type == 3)
                {
                    type = suitTypeSelect.Dragon;
                    suitID = suitID - 4;
                } 

                if (suitID == _GameMaster.tile144[i].suitID)
                if ( (int)type == (int)_GameMaster.tile144[i].myType)
                    {
                        _temp = _GameMaster.tile144[i].tileID;
                        return _temp;
                    }
            }

            else
            {
                Debug.Log("?");
            }
        }

        return _temp;
    }

    public void SetNumberCanvas (bool isNumber)
    {
        NumbersButtons.SetActive(isNumber);
        WindsButtons.SetActive(!isNumber);
    }

    public void setButton ( TheButton input)
    {
        input.myButton.onClick.AddListener(() => buttonClick(input));
    }

    public void buttonClick(TheButton input)
    {
        Debug.Log(input.id);

        if (input.id == 11)
        {
            showText.text = "Cleared : Enter again";
            _GameMaster.clear17();
            _GameMaster.show17();
        }

        if (input.id == 21)
        {
            if (currentType != suitTypeSelect.Dots)
                showText.text = "筒 : Enter tiles you have";
            currentType = suitTypeSelect.Dots;
            SetNumberCanvas(true);
        }

        if (input.id == 22)
        {
            if (currentType != suitTypeSelect.Bams)
                showText.text = "索 : Enter tiles you have";
            currentType = suitTypeSelect.Bams;
            SetNumberCanvas(true);
        }

        if (input.id == 23)
        {
            if (currentType != suitTypeSelect.Chars)
                showText.text = "萬 : Enter tiles you have";
            currentType = suitTypeSelect.Chars;
            SetNumberCanvas(true);
        }

        if (input.id == 24)
        {
            if (currentType != suitTypeSelect.Winds)
                showText.text = "字 : Enter tiles you have";
            currentType = suitTypeSelect.Winds;
            SetNumberCanvas(false);
        }

        if ((input.id >= 1) && (input.id <=9))
        {
            if (currentType != suitTypeSelect.No)
            {
                int tempTile = tileID(input.id , currentType);

                int t = 0;
                for (int i = 0; i < 17; i++)
                {
                    if (_GameMaster.tile17[i].tileID == tempTile)
                    {
                        tempTile++;
                        t++;
                    }

                    if (t == 4)
                    {
                        showText.text = "all 4 tiles entered already";
                        tempTile = 0;
                        break;
                    }
                }

                for (int i = 0; i < 17; i++)
                {
                    if (_GameMaster.tile17[i].tileID == 0)
                    {
                        _GameMaster.tile17[i].tileID = tempTile;
                        _GameMaster.debugShowWins = true;
                        Debug.Log(tempTile);
                        break;
                    }
                }                
            }
        }
    }
}
*/
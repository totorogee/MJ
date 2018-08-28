using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum suitType { Chars, Dots, Bams, Winds, Dragons, Flowers }
public enum meldsType { Pong, Chow, Eyes, Kong, No }
public enum stateType { InHand, Shown, Eaten }
public enum windType { East, South, West, North }


[System.Serializable]
public struct TheTile
{
    public suitType myType;
    public stateType myState;
    public meldsType myMelds;

    [Range(1, 4)]
    public int identityID;
    [Range(0, 144)]
    public int tileID;
    [Range(1, 9)]
    public int suitID;

    public Transform myImage;

}

[System.Serializable]
public struct TheWinComb
{
    public int fan;
    public int[] IdList;
    public int[] sortList;

    [Range(0,9)]
    public int linkedPongTile; // [1] = 111 222 333 
    [Range (0,3)]
    public int linkedPongLevel; // (0 to 3) : [0] = 111 222 333 444 , [1] = 123 123 123 444 , [2] = 111 234 234 234
    public suitType linkedPongOn;

    public int eyeTile;
    public suitType eyeOn;
}


public class GameMaster : MonoBehaviour
{

    public static GameMaster instance;

    public Transform[] tiles = new Transform[36];
    private Vector3 tileSize;
    public TheTile[] tile17 = new TheTile[17];
    public TheTile[] tile144 = new TheTile[145];
    public int[] _13YiuTile = new int[13];

    public Vector3 showPos;
    public Transform _showPos;
    public Vector2 offsetXY;

    public Transform hold144;
    public Transform hold17;
    public Transform holdWin;

    public TheWinComb[] winComb = new TheWinComb[8];
    
    public int eyeWinCom = 0;    
    private int suitStart = 0;
    private suitType eyeOn = suitType.Flowers;

    // debug

    public bool win = false;
    public bool specialLglg = false;
    public bool special13Yiu = false;
    public bool special16NoMatch = false;

    public int lglgPong = 0;


    public int debugMinWin = 3;
    [Range(19, 107)]
    public int debugRange = 35;
    public bool debugWithWind = false;
    public bool debugRandomMutiWin = false;
    public bool debugShowWins = false;

    public int debugLoopLimit = 10000;
    [HideInInspector]
    public int _debugLoopLimit = 1000;

    void Awake()
    {
        instance = this;        
    } 

    // Use this for initialization
    void Start()
    {
        set144();
        clear17();

        showPos = _showPos.position;
        show17();
    }

    // Update is called once per frame
    void Update()
    {

        if (debugRandomMutiWin)
        {
            eyeWinCom = 0;
            int _debugLoopLimit = debugLoopLimit;
            while ((eyeWinCom < debugMinWin) || (!win))
            {
                random17(debugRange , debugWithWind , true);
                win = checkWin();
                sortWinCom();
                _debugLoopLimit--;
                if (_debugLoopLimit < 1)
                {
                    Debug.Log("no match");
                    break;
                }
            }
            Debug.Log(_debugLoopLimit);
            debugRandomMutiWin = false;
            showWinCom();
        }

        if (debugShowWins)
        {
            debugShowWins = false;
            win = checkWin();
            sortWinCom();
            showWinCom();
        }
    }

    public void set144()
    {
        for (int i = 0; i < 145; i++)
        {
            tile144[i].myImage = tiles[0];
            tile144[i].tileID = i;
        }

        for (int i = 1; i <= 9; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                int n = (i - 1) * 4 + j;
                tile144[n].myImage = tiles[i];
                tile144[n].identityID = j;
                tile144[n].myType = suitType.Chars;
                tile144[n].suitID = i;
            }
        }

        for (int i = 10; i <= 18; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                int n = (i - 1) * 4 + j;
                tile144[n].myImage = tiles[i];
                tile144[n].identityID = j;
                tile144[n].myType = suitType.Dots;
                tile144[n].suitID = i - 9;
            }
        }

        for (int i = 19; i <= 27; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                int n = (i - 1) * 4 + j;
                tile144[n].myImage = tiles[i];
                tile144[n].identityID = j;
                tile144[n].myType = suitType.Bams;
                tile144[n].suitID = i - 18;
            }
        }

        for (int i = 28; i <= 31; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                int n = (i - 1) * 4 + j;
                tile144[n].myImage = tiles[i];
                tile144[n].identityID = j;
                tile144[n].myType = suitType.Winds;
                tile144[n].suitID = i - 27;
            }
        }

        for (int i = 32; i <= 34; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                int n = (i - 1) * 4 + j;
                tile144[n].myImage = tiles[i];
                tile144[n].identityID = j;
                tile144[n].myType = suitType.Dragons;
                tile144[n].suitID = i - 31;
            }
        }
    }

    public void show144()
    {
        if (hold144 != null)
            GameObject.Destroy(hold144.gameObject);

        GameObject T = new GameObject();
        hold144 = T.transform;

        for (int i = 0; i < 144; i++)
        {
            Vector3 tempPos = showPos;
            Transform Temp;
            tempPos.x = showPos.x + Mathf.FloorToInt(i / 4) * offsetXY.x;
            tempPos.y = showPos.y + i % 4 * offsetXY.y;
            Temp = Instantiate(tile144[i + 1].myImage, tempPos, Quaternion.identity);
            Temp.SetParent(hold144);
        }
    }


    public void clear17()
    {
        for (int i = 0; i < 17; i++)
        {
            tile17[i] = tile144[0];
        }
    }

    public void show17()
    {
        if (hold17 != null)
            GameObject.Destroy(hold17.gameObject);

        GameObject T = new GameObject();
        hold17 = T.transform;

        for (int i = 0; i < 17; i++)
        {
            tile17[i] = tile144[tile17[i].tileID];

            Vector3 tempPos = showPos;
            Transform Temp;
            tempPos.x = showPos.x + i * offsetXY.x;
            tempPos.y = showPos.y -0.8f;
            Temp = Instantiate(tile17[i].myImage, tempPos, Quaternion.identity);
            Temp.SetParent(hold17);
        }
        hold17.localScale = _showPos.localScale;

    }

    public void showWinCom()
    {
        if (holdWin != null)
            GameObject.Destroy(holdWin.gameObject);

        GameObject T = new GameObject();
        holdWin = T.transform;

        for (int i = 0; i < eyeWinCom; i++)
        {
            for (int j = 0; j < 17; j++)
            {
                Vector3 tempPos = showPos;
                Transform Temp;
                tempPos.x = showPos.x + j * offsetXY.x;
                tempPos.y = showPos.y + i * offsetXY.y;
                tempPos.x = tempPos.x + Mathf.FloorToInt((j + 1) / 3) * offsetXY.y / 4;
                Temp = Instantiate(tile144[winComb[i].IdList[j]].myImage, tempPos, Quaternion.identity);
                Temp.SetParent(holdWin);
            }            
        }

        if (specialLglg)
        {
            eyeWinCom += 1;
            for (int j = 0; j < 17; j++)
            {
                Vector3 tempPos = showPos;
                Transform Temp;
                tempPos.x = showPos.x + j * offsetXY.x;
                tempPos.y = showPos.y + (eyeWinCom -1 )* offsetXY.y;

                if (j <= lglgPong -1)
                    tempPos.x = tempPos.x + Mathf.FloorToInt(j / 2) * offsetXY.y / 4;

                if (j >= lglgPong )
                    tempPos.x = tempPos.x + Mathf.FloorToInt((j - 1) / 2) * offsetXY.y / 4;

                Temp = Instantiate(tile144[tile17[j].tileID].myImage, tempPos, Quaternion.identity);
                Temp.SetParent(holdWin);
            }
        }

        holdWin.localScale = _showPos.localScale;
    }

    public void random17(int limit, bool wind , bool bias) // ramdom 17 suit ( if bias = true , random a winning suits; wind = have wind suits)
    {
        if (limit > 107)
            limit = 107;

        if (wind)
            limit += 28;

        if(!wind)
            if (limit < 27)
                limit = 27;

        int[] n = new int[20];
        for (int i = 0; i < 17; i++)
        {
            bool b = true;

            if ((i >= 15) && (bias))
                while (b)
                {
                    b = false;
                    int x = Random.Range(1, limit);

                    if (wind)
                        if (x + 28 > limit)
                            x = (x + 136) - limit;

                    n[i] = x;
                    if (x + 1 >= limit)
                        n[i + 1] = x - 1;
                    else
                        n[i + 1] = x + 1;

                    for (int j = 0; j < i; j++)
                    {
                        if (n[i] == n[j])
                            b = true;
                        if (n[i+1] == n[j])
                            b = true;
                    }

                    if (!b)
                    {
                        tile17[i] = tile144[n[i]];
                        tile17[i+1] = tile144[n[i+1]];
                        i += 1;
                    }
                }

            if ((i <= 14) && (bias))
            while (b)
            {
                b = false;
                int z = 1;
                int x = Random.Range(1, limit);

                if (wind)
                    if (x + 28 > limit)
                          x = (x + 136) - limit;

                int y = Random.Range(0, 8);
                if (y == 0)
                    {
                        n[i] = x;
                        if (x + 2 >= limit)
                            z = -1;
                        n[i + 1] = x + 1*z;
                        n[i + 2] = x + 2*z;
                    }
                else
                    {
                        n[i] = x;
                        if (x + 8 >= limit)
                            z = -1;
                        n[i + 1] = x + 4*z;
                        n[i + 2] = x + 8*z;
                    }


                for (int k = 0; k <3; k++)
                    for (int j = 0; j < i; j++)
                    {
                        if (n[i+k] == n[j])
                            b = true;
                    }

                if (!b)
                    {
                        tile17[i] = tile144[n[i]];
                        tile17[i + 1] = tile144[n[i + 1]];
                        tile17[i + 2] = tile144[n[i + 2]];
                        i += 2;
                    }
            }
            

            if (!bias)
            while (b)
            {
                b = false;
                int x = Random.Range(1, limit);

                if (wind)
                    if (x + 28 > limit)
                         x = (x + 136) - limit;

                n[i] = x;

                for (int j = 0; j < i; j++)
                {
                    if (x == n[j])
                        b = true;
                }

                if(!b)
                    tile17[i] = tile144[n[i]];
            }
        }
    }

    public void sort17()
    {
        int[] n = new int[17];
        int x = 0;
        for (int j = 0; j <= 144; j++)
        {
            for (int i = 0; i < 17; i++)
            {
                if (tile17[i].tileID == j)
                {
                    n[x] = j;
                    x++;
                }
            }
        }

        for (int i = 0; i < 17; i++)
        {
            tile17[i] = tile144[n[i]];

        }
        show17();
    }

    public void sortWinCom() 
    {
        for (int j = 0; j < eyeWinCom; j++)
        {

            for (int i = 0; i < 17; i++)
            {
                winComb[j].IdList[i] = tile17[i].tileID;
                winComb[j].sortList[i] = i+1;
            }

            for (int i = 0; i < 17; i++)
            {
                if (tile144[winComb[j].IdList[i]].myType == winComb[j].eyeOn)
                if (tile144[winComb[j].IdList[i]].suitID == winComb[j].eyeTile)
                    {
                        sortSuit(winComb[j].sortList, i, 0);
                        sortSuit(winComb[j].IdList, i, 0);
                        sortSuit(winComb[j].sortList, i + 1, 1);
                        sortSuit(winComb[j].IdList, i + 1, 1);
                        break;
                    }
            }

            if (winComb[j].linkedPongLevel == 0) // check if there is any 111 222 333
                checkLinkedPong(j); // check and set linkedPongLevel to != 0 and add one to eyeWinCom

            for (int i = 2; i < 15 ; i=i+3)
            {
                bool asChow = true;

                if (tile144[winComb[j].IdList[i]].suitID == tile144[winComb[j].IdList[i + 2]].suitID)
                {
                    asChow = false;

                    if ((winComb[j].linkedPongLevel >= 1)&& (winComb[j].linkedPongOn == tile144[winComb[j].IdList[i]].myType))
                        if (tile144[winComb[j].IdList[i]].suitID == winComb[j].linkedPongTile + (winComb[j].linkedPongLevel - 1))
                            asChow = true;
                }
                
                if (asChow == true)
                {
                    sortSuit(winComb[j].sortList, i, i);
                    sortSuit(winComb[j].IdList, i, i);

                    for (int k = i;  k < 17;  k++)
                    {
                        if (tile144[winComb[j].IdList[i]].suitID +1 == tile144[winComb[j].IdList[k]].suitID)
                        {
                            sortSuit(winComb[j].sortList, k, i + 1);
                            sortSuit(winComb[j].IdList, k, i + 1);
                            break;
                        }
                    }

                    for (int k = i; k < 17; k++)
                    {
                        if (tile144[winComb[j].IdList[i]].suitID +2 == tile144[winComb[j].IdList[k]].suitID)
                        {
                            sortSuit(winComb[j].sortList, k, i + 2);
                            sortSuit(winComb[j].IdList, k, i + 2);
                            break;
                        }
                    }
                }
            }
        }

        bool _checkS = true;
        for (int i = 0; i < 17; i++)
        {
            if (tile17[i].tileID == 0)
            {
                _checkS = false;
            }
        }

        if(_checkS)
        checkSpecialCombo();
    }

    // check if suit have 111 222 333 for winning combo[n]
    public void checkLinkedPong(int n) 
    {
        int j = 0;
        int _x = 0; // count Linked Level
        int _y = 0; // store last pong
        suitType _On = suitType.Flowers; // checking which suit

        int linkedTile = 0; //result tile
        int linkedLevel = 0; // result level
        suitType linkedOn = suitType.Flowers;

        for (int i = 2; i < 15; i++)
        {
            // not wind
            if (tile144[winComb[n].IdList[i]].myType <= (suitType)2)
            {
                // have Pong
                if (tile144[winComb[n].IdList[i]].suitID == tile144[winComb[n].IdList[i + 2]].suitID)
                {
                    j = tile144[winComb[n].IdList[i]].suitID;


                    // check if it can win without the 3 tiles
                    int[] temp = new int[winComb[n].IdList.Length - 5];
                    int m = 0;
                    while (m < temp.Length)
                    {
                        if (m < i - 2)
                            temp[m] = tile144[winComb[n].IdList[m + 2]].suitID;

                        if (m >= i -2 )
                            temp[m] = tile144[winComb[n].IdList[m + 5]].suitID;

                        m++;
                    }
                                       
                    if (!checkWin(temp, false, false))
                        _x = 0;

                    else
                    {                           
                        // last suit number have Pong
                        if ((j == _y + 1) && (tile144[winComb[n].IdList[i]].myType == _On))
                        {

                            _x++;
                                                        
                            // x = 3 --> have 111 222 333
                            if (_x == 3)
                            {
                                linkedTile = j - 2;
                                linkedOn = _On;
                            }

                            // x >= 3 --> have 111 222 333 (444) (555)
                            if (_x >= 3)
                                linkedLevel = _x - 2;
                        }

                        else
                            _x = 1;
                    }

                    if (winComb[n].IdList.Length > i+3 )
                    if ((tile144[winComb[n].IdList[i]].suitID == tile144[winComb[n].IdList[i + 3]].suitID))
                        i++;

                    _y = j;
                    i = i + 2;
                    _On = tile144[winComb[n].IdList[i]].myType;
                }
            }           
        }

        for (int i = 0; i < linkedLevel; i++)
        {
            winComb[eyeWinCom + i].eyeOn = winComb[n].eyeOn;
            winComb[eyeWinCom + i].eyeTile = winComb[n].eyeTile;

            winComb[eyeWinCom + i].linkedPongLevel = i + 1;
            winComb[eyeWinCom + i].linkedPongTile = linkedTile;
            winComb[eyeWinCom + i].linkedPongOn = linkedOn;

            for (int k = 0; k < 17; k++)
            {
                winComb[eyeWinCom + i].IdList[k] = tile17[k].tileID;
                winComb[eyeWinCom + i].sortList[k] = k + 1;
             }
        }
            eyeWinCom += linkedLevel;
    }

    public void checkSpecialCombo()
    {
        // ----------- check Lglg -----------
        int pong = 0;
        specialLglg = true;

        for (int i = 0; i < 17; i++)
        {
            if ((i <= 15 ) && (tile17[i].suitID == tile17[i + 1].suitID) && (tile17[i].myType == tile17[i + 1].myType))
                i += 1;
            else if ((i >= 1) && (tile17[i].suitID == tile17[i - 1].suitID) && (tile17[i].myType == tile17[i - 1].myType))
            {
                pong += 1;
                lglgPong = i;
            }
            else
                specialLglg = false;            
        }

        if (pong != 1)
            specialLglg = false;

        // ----------- check 13yiu ----------- 
        special13Yiu = false;
        int j = 0;
        int k = 0;
        int m = 0;
        int[] tempWinSort = new int[17];
        int[] temp2 = new int[3];

        while (j <17)
        {
            if ((tile17[j].tileID >= _13YiuTile[k]) && (tile17[j].tileID <= _13YiuTile[k] + 3) && (k < 13))
                {
                    tempWinSort[k] = tile17[j].tileID;
                    k++;
                }

            else
                {
                    if (m <= 3)
                    {
                        tempWinSort[13 + m] = tile17[j].tileID;
                        m++;
                    }
                    else
                        break;
                }            
            j++;
        }

        if ((k == 13) && (m == 4))
        {
            // check all 4 tiles 
            for (int i = 0; i < 4; i++)
            {
                // make a list without one of that tile
                for (int x = 0; x < 4; x++)
                {
                    if (x < i)
                        temp2[x] = tempWinSort[13 + x];

                    if (x > i)
                        temp2[x - 1] = tempWinSort[13 + x];
                }

                // check if it win
                for (int n = 0; n < 13; n++)
                {
                    bool wind = true;
                    if (tile144[temp2[0]].myType <= (suitType)2)
                        wind = false;

                    if ((tempWinSort[13 + i] >= _13YiuTile[n]) && (tempWinSort[13 + i] <= _13YiuTile[n] + 3))
                        special13Yiu = checkWin(temp2, wind, false);
                }
            }
        }

        // ----------- check 16 no match ----------- 
        special16NoMatch = false;
        j = 0;
        k = 0;
        m = 0;
        
        // check and sort ( 0 to 6 = winds , 7 to 15 unsorted , 16 eye )
        bool _eye = false;
        while (j < 17)
        {
            // check if it have one eye
            if (j < 16)
            if ((tile17[j].suitID == tile17[j + 1].suitID) && (tile17[j].myType == tile17[j + 1].myType))
            {
                if (_eye)
                {
                    m = 0;
                    break;
                }
                _eye = true;
                tempWinSort[16] = tile17[j].tileID;

                }

            // check if it match tile 6 - 12 of 13yiu
            if ((tile17[j].tileID >= _13YiuTile[k+6]) && (tile17[j].tileID <= _13YiuTile[k+6] + 3) && (k < 7))
            {
                tempWinSort[k] = tile17[j].tileID;
                k++;
            }

            else
            {
                if (m <= 9)
                {
                    tempWinSort[7 + m] = tile17[j].tileID;
                    m++;
                }
                else
                    break;
            }
            j++;
        }

        // if the 0 - 6 , 16 is checked, check 7 - 15
        if ((k == 7) && (m == 10))
        {
            Debug.Log("7 match");
            bool _tempB = true;

            // check each suitType
            for (int i = 0; i < 3; i++)
            {
                // check 3 tiles of each suitType
                for (int p = 0; p < 3; p++)
                {
                    int x = 7 + (i * 3) + p;
                    Debug.Log(tempWinSort[x]);

                    if (tile144[tempWinSort[x]].myType != (suitType)i)
                    {
                        _tempB = false;
                        break;
                    }

                    if (p == 1)
                    if ((tile144[tempWinSort[x]].suitID - tile144[tempWinSort[x - 1]].suitID < 3) && (tile144[tempWinSort[x + 1]].suitID - tile144[tempWinSort[x]].suitID < 3))
                    {
                        _tempB = false;
                        break;
                    }
                }
            }
            special16NoMatch = _tempB;
        }
    }

    // put "a" to "pos" , move the tile before "a" one step forward
    public void sortSuit(int[] list, int a, int pos)     
    {
        int temp = list[a];
        for (int i = a; i > pos; i--)
        {
            list[i] = list[i - 1];
        }
        list[pos] = temp;
    }

    // reset winning combo count
    public void resetWinCom() 
    {
        eyeWinCom = 0;

        for (int k = 0; k < 6; k++)
        {
            winComb[k].IdList = new int[17];
            winComb[k].sortList = new int[17];

            winComb[k].linkedPongLevel = 0;
            winComb[k].linkedPongTile = 0;
            winComb[k].linkedPongOn = suitType.Flowers;

            for (int m = 0; m < winComb[k].IdList.Length; m++)
            {
                winComb[k].IdList[m] = 0;
                winComb[k].sortList[m] = m + 1;
            }
        }
    }

    // check if tile17 win
    public bool checkWin()
    {
        bool eyeEd = false;
        int[] suitNumber = new int[5];

        sort17();
        resetWinCom();

        for (int i = 0; i < 5; i++)
            suitNumber[i] = 0;
        

        for (int i = 0; i < 17; i++)
        {
            if ((int)(tile17[i].myType) <= 4)
                suitNumber[(int)(tile17[i].myType)]++;
            else
                return false;

            if (tile17[i].tileID == 0)
                return false;

            tile17[i].myMelds = meldsType.No;
        }

        // find what could be the eye suitType
        for (int i = 0; i < 5; i++)
        {
            if (suitNumber[i] % 3 == 1)
                return false;
            
            if (suitNumber[i] % 3 == 2)
                if (eyeEd)
                    return false;
                else
                {
                    eyeEd = true;
                    eyeOn = (suitType)i;
                }
        }

        // shown need to check !!

        int x = 0;
        while (x < 17)
        {
            bool wind = true;
            bool eye = true;

            if (tile17[x].myType <= (suitType)2)
                wind = false;
            else if (tile17[x].myType <= (suitType)5)
                wind = true;
            
            // check if the suitType could have eye
            eye = (eyeOn == tile17[x].myType);

            int[] checkList = new int[suitNumber[(int)tile17[x].myType]];
            for (int i = 0; i < checkList.Length; i++)
                checkList[i] = tile17[x + i].suitID;

            suitStart = x; // starting of a suit ( use a public ? )

            // go to check one suit at a time
            if (checkWin(checkList, wind, eye) == false)
                return false;

            x = x + checkList.Length;
        }
        return true;
    }

    // check one suit at a time
    public bool checkWin(int[] list, bool wind, bool eye)
    {
        // no tile in any suit count as pass
        if (list.Length == 0)
            return true; 

        // check as eye is included
        if (eye)
        {
            bool check = false;
            for (int i = 0; i < list.Length - 1; i++)
            {
                if (list[i] == list[i + 1])
                {
                    tile17[suitStart + i].myMelds = meldsType.Eyes;
                    tile17[suitStart + i+1].myMelds = meldsType.Eyes;

                    // make a list without the eye
                    int[] temp = new int[list.Length - 2];
                    for (int j = 0; j < list.Length; j++)
                    {
                        if (j < i)
                            temp[j] = list[j];
                        if (j > i + 1)
                            temp[j-2] = list[j];
                    }

                    // check the list as the eye removed
                    if (checkWin(temp, wind, false))
                    {
                        check = true;

                        winComb[eyeWinCom].eyeTile = list[i];
                        winComb[eyeWinCom].eyeOn = eyeOn;

                        eyeWinCom++;
                    }

                    // only check once for eye if there 3 or 4 are same tiles 
                    while ((i+1 <list.Length) && (list[i] == list[i+1])) 
                    i++;
                }
            }

            if (check == false)
                return false;
        }

        // check as eye is not included ( Wind suit )
        if (wind && !eye)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (i % 3 == 0)
                {
                    if (list[i] != list[i + 2])
                    {
                        return false;
                    }
                    i = i + 2;
                }

                tile17[suitStart + i].myMelds = meldsType.Pong;
            }  
        }

        // check as eye is not included ( not wind suit)
        if (!wind && !eye)
        {
            bool check = false;

            // check if it fix as all "chow"
            if (checkChow(list))
                check = true;
            
            // if can't, remove a Pong and check again
            else
            {
                for (int i = 0; i < list.Length - 2; i++)
                {
                    if (list[i] == list[i + 2])
                    {
                        // pick out a Pong and make a new list
                        int[] temp = new int[list.Length - 3];
                        for (int j = 0; j < list.Length ; j++)
                        {
                            if (j < i)
                                temp[j] = list[j];
                            if (j > i + 2)
                                temp[j-3] = list[j];
                        }

                        // check the new list
                        if (checkWin(temp, false, false))
                            check = true;
                        i = i + 2;
                    }
                }
            }

            if (check == false)
                return false;
        }
        return true; // no use 
    }

    // check if the list can sort into all "chow" [ 123 345 567]
    public bool checkChow(int[] list)
    {
        // x store 1st tile have suit number of first of list + 1
        // y store 1st tile have suit number of first of list + 2

        int x = 0;
        int y = 0;
        bool check = false;

        for (int i = 1; i < list.Length; i++)
        {
            if ((list[0] + 1 == list[i] ) && (check == false))
            {
                x = i;
                for (int j = i+1; j < list.Length; j++)
                {
                    if ((list[0]+2 == list[j]) && (check == false))
                    {
                        y = j;
                        check = true;
                    }
                }
            }
        }

        if (check)
        {
            if (list.Length == 3)
                return true;

            if (list.Length > 3)
            {
                int[] temp = new int[list.Length - 3];
                int n = 0;

                // pick out the 3 tiles from the list
                for (int i = 1; i < list.Length; i++)
                { 
                    if ((i != x) && (i != y))
                    {
                        temp[n] = list[i];
                        n++;
                    }
                }

                // input the new list and check again
                if (checkChow(temp))
                    return true;
            }
        }

        return false;
    }
}

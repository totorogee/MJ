using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCheck
{
    public List<List<Tiles>> combinations = new List<List<Tiles>> ();
    private List<Tiles> tiles = new List<Tiles>();

    private bool foundEyesInSuit = false;
    private SuitType suitWithEyes = SuitType.Nil;

    private List<Tiles> checkedTiles = new List<Tiles>();
    private List<Tiles> unCheckedTiles = new List<Tiles>();

    private int[] numberOfTilesInSuit = new int[6];
    private int[,] numberOfRankInSuit = new int[6, 10];
    private SuitType linkedTripletAtSuit;
    private int linkedTripletAtRank;
    private int linkedTripletLevel = 0;

    private int[,] unCheckedNumberOfRanksInSuit = new int[6, 10];

    public bool IsWin = false;

    public WinCheck(List<Tiles> tiles)
    {
        this.tiles = tiles;
        CheckRankOfAllTiles();
        CheckWin();
    }

    #region Functions to Check if it is winning hands

    // Starting point : picking out pairs as eye and check the rest 
    public bool CheckWin() 
    {
        IsWin = false;
        SortTiles();

        if (tiles.Count != 17)
        {
            return false;
        }

        if (numberOfTilesInSuit[0] > 0 || numberOfTilesInSuit[5] > 0)
        {
            return false;
        }

        for (int i = 1; i < 5; i++)
        {
            switch (numberOfTilesInSuit[i] % 3)
            {
                case 1:
                    return false;
                case 2:
                    if (foundEyesInSuit)
                    {
                        return false;
                    }
                    foundEyesInSuit = true;
                    suitWithEyes = (SuitType)i;
                    break;
                default:
                    break;
            }
        }

        for (int i = 1; i <= 9; i++)
        {
            if (numberOfRankInSuit[(int)suitWithEyes, i] >= 2)
            {
                unCheckedTiles.Clear();
                foreach (var item in tiles)
                {
                    unCheckedTiles.Add(item);
                }

                for (int j = 0; j < numberOfRankInSuit.GetLength(0); j++)
                {
                    for (int k = 0; k < numberOfRankInSuit.GetLength(1); k++)
                    {
                        unCheckedNumberOfRanksInSuit[j, k] = numberOfRankInSuit[j, k];
                    }
                }

                checkedTiles.Clear();
                linkedTripletLevel = 0;

                MarkTileAsCheck(suitWithEyes, i);
                MarkTileAsCheck(suitWithEyes, i);

                if (CheckWinWithoutEye())
                {
                    IsWin = true;

                    List<Tiles> temp = new List<Tiles>();
                    foreach (var item in checkedTiles)
                    {
                        temp.Add(item);
                    }
                    combinations.Add(temp);

                    if (linkedTripletLevel >= 3)
                    {
                        for (int n = 0; n <= (linkedTripletLevel - 3); n++)
                        {
                            combinations.Add(LinkedTripletSwitch(checkedTiles, n));
                        }
                    }
                }
            }
        }
        return IsWin;
    }

    // Check if the tiles without eye can from combinations
    public bool CheckWinWithoutEye()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (CheckWinWithoutEye((SuitType)i) == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckWinWithoutEye(SuitType suit)
    {
        int i = 0;
        int debugTry = 0;
        while (true)
        {
            debugTry++;

            switch (unCheckedNumberOfRanksInSuit[(int)suit, i])
            {
                case 0:
                    i++;
                    break;
                case 1:
                case 2:
                    if (!TryMarkAsSequence(suit, i))
                    {
                        return false; 
                    }
                    break;
                case 3:
                case 4:
                    int linked = TryMarkAsLinkedTriplet(suit, i);
                    if (linked == 0)
                    {
                        return false;
                    }
                    if (linked >= 3)
                    {
                        linkedTripletLevel = linked;
                        linkedTripletAtSuit = suit;
                        linkedTripletAtRank = i;
                    }
                    break;
            }

            if (i > 9)
            {
                break;
            }

            if (debugTry > 1000)
            {
                Debug.Log("Looped  " + i);
                return false;
            }
        }

        for (int n = 1; n <= 9; n++)
        {
            if (unCheckedNumberOfRanksInSuit[(int)suit, n] != 0)
            {
                return false;
            }
        }

        return true;
    }

    // Picking out Meld ( Sequence or Triplets) 3 tiles at a time
    private bool TryMarkAsSequence(SuitType suit, int rank)
    {
        if (rank >= 8)
        {
            return false;
        }

        if (suit != SuitType.Bams && suit != SuitType.Chars && suit != SuitType.Chars)
        {
            return false;
        }

        bool found = true;
        for (int i = 0; i <= 2; i++)
        {
            if (!MarkTileAsCheck(suit, rank + i, true))
            {
                found = false;
            }
        }

        if (!found)
        {
            return false;
        }
        else
        {
            MarkTileAsCheck(suit, rank);
            MarkTileAsCheck(suit, rank + 1);
            MarkTileAsCheck(suit, rank + 2);
            return true;
        }
    }

    private bool TryMarkAsTriplet(SuitType suit, int rank)
    {
        if (unCheckedNumberOfRanksInSuit[(int)suit, rank] >= 3)
        {
            MarkTileAsCheck(suit, rank);
            MarkTileAsCheck(suit, rank);
            MarkTileAsCheck(suit, rank);
            return true;
        }
        return false;
    }

    // Picking out special case such as ( 555 666 777 ) Bugged
    private int TryMarkAsLinkedTriplet(SuitType suit, int rank)
    {
        if (suit == SuitType.Honor)
        {
            if (TryMarkAsTriplet(suit, rank))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        int link = 0;
        for (int i = rank; i < 9; i++)
        {
            if (TryMarkAsTriplet(suit, i))
            {
                link++;
            }
            else
            {
                Debug.Log("here " + link);
                return link;
            }
        }
        return link;
    }

    #endregion

    #region Functions for instantiate

    private void CheckRankOfAllTiles()
    {
        foreach (Tiles tile in tiles)
        {
            this.numberOfRankInSuit[(int)tile.GetSuitType(), tile.GetRank()]++;
            this.numberOfTilesInSuit[(int)tile.GetSuitType()]++;
        }
    }

    private void SortTiles()
    {
        if (tiles == null || tiles.Count <= 1)
        {
            Debug.Log("error");
            return;
        }

        tiles.Sort(delegate (Tiles x, Tiles y)
        {
            if (x.TileID == y.TileID)
            {
                return 1;
            }
            else
            {
                return x.TileID.CompareTo(y.TileID);
            }
        });
    }

    #endregion

    // Sorting the winning combination a tile at a time
    private bool MarkTileAsCheck(SuitType suit, int rank, bool onlyCheck = false)
    {
        for (int i = 0; i < unCheckedTiles.Count; i++)
        {
            if (unCheckedTiles[i].GetRank() == rank && unCheckedTiles[i].GetSuitType() == suit)
            {
                if (!onlyCheck)
                {
                    Tiles tile = unCheckedTiles[i];
                    unCheckedTiles.RemoveAt(i);
                    checkedTiles.Add(tile);

                    unCheckedNumberOfRanksInSuit[(int)suit, rank]--;
                    if (unCheckedNumberOfRanksInSuit[(int)suit, rank] < 0)
                    {
                        Debug.Log("Error in unchecked list");
                    }
                }
                return true;
            }
        }

        if (!onlyCheck)
        {
            Debug.Log("Error : Tiles Not Found");
        }

        return false;
    }

    // Sorting special case ( 111 222 333 ) to ( 123 123 123 )
    // Both combination is valid have to show separately
    private List<Tiles> LinkedTripletSwitch(List<Tiles> tiles, int n)
    {
        List<Tiles> result = new List<Tiles>();

        int pos = 0;

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].GetSuitType() == linkedTripletAtSuit && tiles[i].GetRank() == linkedTripletAtRank + n)
            {
                pos = i;
                break;
            }
        }

        Debug.Log(linkedTripletAtSuit + " " + linkedTripletAtRank + " " + pos);

        for (int j = 0; j < tiles.Count; j++)
        {
            if (j >=pos && j <= pos + 8)
            {
                int k = j - pos;
                k = k / 3 + k % 3 * 3 + pos;  // j-pos : 012 345 678 => k : 036 147 258
                result.Add(tiles[k]);
            }
            else
            {
                result.Add(tiles[j]);
            }
        }
        return result;
    }


}

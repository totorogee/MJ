using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tiles
{
    public int TileID = -1;
    public static List<Sprite> Icons;
    public static Sprite ClosedIcon;
    public StateType MyState = StateType.Nil;
    public bool Closed = true;

    public Tiles(int TileID)
    {
        this.TileID = TileID;
    }

    public HonorType GetHonorType()
    {
        if(GetSuitType() == SuitType.Honor)
        {
            return (HonorType)GetRank();
        }
        return HonorType.Nil;
    }

    public SuitType GetSuitType()
    {
        switch ((TileID -1 ) /36)
        {
            case 0:
                return SuitType.Chars;
            case 1:
                return SuitType.Dots;
            case 2:
                return SuitType.Bams;
            default:
                if( (TileID -1) /4 + 1 >= 28 && (TileID - 1) /4 + 1 <= 34)
                {
                    return SuitType.Honor;
                }
                if ((TileID - 1) / 4 + 1 == 35 || (TileID - 1) / 4 + 1 == 36  )
                {
                    return SuitType.Flowers;
                }
                return SuitType.Nil;
        }
    }

    public int GetRank() // 1 to 9
    {
        if ((int)GetSuitType() <= 4 && GetSuitType() != SuitType.Nil)
        {
            return ( ( (TileID -1 ) / 4) % 9 + 1 );
        }
        if (GetSuitType() == SuitType.Flowers)
        {
            return (TileID - 136);
        }
        return -1;
    }

}


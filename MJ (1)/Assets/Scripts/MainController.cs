using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Reference : https://www.xqbase.com/other/mahjongg_english.htm
public enum SuitType { Nil, Chars, Dots, Bams, Honor, Flowers }
public enum MeldsType { Nil, Eyes, Triplet, Sequence, ExposedKong, ConcealedKong }
public enum StateType { Nil, InHand, Shown, WinningTile }
public enum HonorType { Nil, East, South, West, North, White, Green, Red }

public class MainController : MonoBehaviour {
    public static MainController Instance;
    public static string IconPath = "Sprites/Icon";
    public static string PerfabsPath = "Perfabs";

    public List<Tiles> Main17 = new List<Tiles>(17);
    public List<Tiles> Tile = new List<Tiles>(145);
    public bool stop = false;
    public int trial = 0;
    public bool shown = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        Tiles.Icons = new List<Sprite>( Resources.LoadAll<Sprite>(IconPath) );
        Tiles.ClosedIcon = Resources.Load<Sprite>(IconPath + "MJTiles-0");

        for (int i = 0; i < 145; i++)
        {
            Tiles t = new Tiles(i);
            Tile.Add(t);
        }
        Debug.Log(DateTime.Now);
        Debug.Log(DateTime.Now - new DateTime(2000, 1, 1));
        Debug.Log( (int) (DateTime.Now - new DateTime(2000, 1, 1)).TotalSeconds );
        Debug.Log(int.MaxValue);
    }

    private void Update()
    {


        if (!stop)
        {
            Main17 = Generate17(true);

            WinCheck win = new WinCheck(Main17);
            Debug.Log(win.IsWin);

            if (win.IsWin)
            {
                stop = true;
            }

            trial++;

            if (trial > 100)
            {
                stop = true;
            }
        }

        if (stop && !shown)
        {
            foreach (var item in Main17)
            {
                Debug.Log(item.GetSuitType() + "  " + item.GetRank());
            }
            shown = true;
        }

    }

    private void ShowOne(Tiles tile , Vector3 position, Transform parent = null)
    {
        GameObject go = new GameObject();
        go.transform.position = position;
        if (parent != null)
        {
            go.transform.SetParent(parent);
        }
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        if (tile.TileID/4 < Tiles.Icons.Count)
        {
            sr.sprite = Tiles.Icons[tile.TileID/4];
        }
        else
        {
            sr.sprite = Tiles.ClosedIcon;
        }
    }

    private List<Tiles> Generate17(bool random = false)
    {
        bool duplicate = false;
        int n = 0;

        List<Tiles> Result = new List<Tiles>();
        for (int i = 0; i < 17; i++)
        {
            Result.Add(new Tiles(0));
        }

        if (random == true)
        {
            for (int i = 0; i < 17; i++)
            {
                do
                {
                    duplicate = false;
                    n = UnityEngine.Random.Range(1, 36);
                    foreach (Tiles tile in Result)
                    {
                        if (tile.TileID == n)
                        {
                            duplicate = true;
                        }
                    }
                }
                while (duplicate);
                Result[i].TileID = n;
            }
        }

        SortTiles(Result);
        return Result;
    }

    private void SortTiles (List<Tiles> tiles)
    {
        if (tiles == null || tiles.Count <= 1)
        {
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
}

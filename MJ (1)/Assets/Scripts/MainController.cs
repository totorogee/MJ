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

    public WinCheck win;

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

        win = new WinCheck(Generate17(true));

    }

    // Use this for initialization
    void Start ()
    {
        Tiles.Icons = new List<Sprite>( Resources.LoadAll<Sprite>(IconPath) );
        Tiles.ClosedIcon = Resources.Load<Sprite>(IconPath + "MJTiles-32");

        for (int i = 0; i < 145; i++)
        {
            Tiles t = new Tiles(i);
            Tile.Add(t);
        }
    //    Debug.Log(DateTime.Now);
    //    Debug.Log(DateTime.Now - new DateTime(2000, 1, 1));
     //   Debug.Log( (int) (DateTime.Now - new DateTime(2000, 1, 1)).TotalSeconds );
     //   Debug.Log(int.MaxValue);
    }

    private void Update()
    { 
        if (!stop)
        {
            Main17 = Generate17(true);
            win = new WinCheck(Main17);

            if( (win.IsWin) && (win.combinations.Count >=3))
            {
                stop = true;
            }

            trial++;

            if (trial > 1000)
            {
                stop = true;
            }
        }

        if (stop && !shown)
        { 
            Show(Main17);
            Show(win.combinations);
            shown = true;
        }
    }

    public void Show(List<Tiles> tiles, float row = 0f)
    {
        float x = Tiles.Icons[0].bounds.size.x * 1.1f;
        float y = Tiles.Icons[0].bounds.size.y * 1.1f;

        for (int i = 0; i < tiles.Count ; i++)
        {
            GameObject go = new GameObject();
            SpriteRenderer sp = go.AddComponent<SpriteRenderer>();
            sp.sprite = tiles[i].GetIcon();

            int space = (i + 1 ) / 3; 
            go.transform.position = new Vector3(x*i + space * x*0.1f, y * row, 0f);
        }
    }

    public void Show(List<List<Tiles>> winningHands)
    {
        float y = 0f;
        foreach (var item in winningHands)
        {
            y++;
            Show(item, y);
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

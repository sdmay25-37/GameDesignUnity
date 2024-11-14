using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmController : MonoBehaviour
{
    private float timer;
    [SerializeField] private float growthTime = 3;

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private Tile[] tiles;

    //x: -8 to 7, y: -4 to -9
    [HideInInspector] public Vector3Int[] farmTiles;

    public void InitFarmtTiles()
    {
        farmTiles = new Vector3Int[96];

        int index = 0;
        for (int x = -8; x <= 7; x++)
        {
            for (int y = -4; y >= -9; y--)
            {
                farmTiles[index] = new Vector3Int(x, y);
                index++;
            }
        }
    }

    private List<Farm> activeTiles;

    private int farmTarget = 0;
    private Color targetColor = new Color(1.0f, 0.7f, 0.7f);

    void Start()
    {
        InitFarmtTiles();
        activeTiles = new List<Farm>();
        timer = growthTime;

        //map.SetTile(new Vector3Int(0, 0), tiles[0]);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        /*
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetTileColor(farmTiles[farmTarget], Color.white);
            farmTarget = (farmTarget + 1) % farmTiles.Length;
            SetTileColor(farmTiles[farmTarget], targetColor);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetTileColor(farmTiles[farmTarget], Color.white);
            farmTarget--;
            if(farmTarget < 0)
            {
                farmTarget = farmTiles.Length - 1;
            }    
            SetTileColor(farmTiles[farmTarget], targetColor);
        }

        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
        */

        FarmUpdate();
    }

    private void FarmUpdate()
    {
        Color color;
        if (timer < 0)
        {
            timer = growthTime;
            for (int i = activeTiles.Count - 1; i >= 0; i--)
            {
                Farm farm = activeTiles[i];

                if (farm.farmstate >= (int)FARMSTATE.FLOWER)
                {
                    activeTiles.RemoveAt(i);
                    continue;
                }

                color = map.GetColor(farm.loc);
                try
                {
                    map.SetTile(farm.loc, tiles[++farm.farmstate]);
                }
                catch (System.Exception)
                {
                    Debug.Log(farm.farmstate);
                    SetTileColor(farm.loc, Color.black);
                    throw;
                }
                SetTileColor(farm.loc, color);
            }
        }
    }

    private void Interact()
    {
        if (map.GetTile(farmTiles[farmTarget]).Equals(tiles[(int)FARMSTATE.EMPTY]))
        {
            activeTiles.Add(new Farm(farmTiles[farmTarget], (int)FARMSTATE.SEED));
            map.SetTile(farmTiles[farmTarget], tiles[(int)FARMSTATE.SEED]);
            SetTileColor(farmTiles[farmTarget], targetColor);

        }
        else if (map.GetTile(farmTiles[farmTarget]).Equals(tiles[(int)FARMSTATE.FLOWER]))
        {
            map.SetTile(farmTiles[farmTarget], tiles[(int)FARMSTATE.EMPTY]);
            SetTileColor(farmTiles[farmTarget], targetColor);
        }
    }

    public void InteractTile(Vector3Int spot)
    {
        //Debug.Log("InteractTile called on position: " + spot);

        if (map.GetTile(spot).Equals(tiles[(int)FARMSTATE.EMPTY]))
        {
            activeTiles.Add(new Farm(spot, (int)FARMSTATE.SEED));
            map.SetTile(spot, tiles[(int)FARMSTATE.SEED]);

        }
        else if (map.GetTile(spot).Equals(tiles[(int)FARMSTATE.FLOWER]))
        {
            map.SetTile(spot, tiles[(int)FARMSTATE.EMPTY]);
        }
    }

    private void SetTileColor(Vector3Int location, Color color)
    {
        map.SetTileFlags(location, TileFlags.None);
        map.SetColor(location, color);
    }
}

public enum FARMSTATE
{
    EMPTY = 0,
    SEED = 1,
    SPROUT = 2,
    YOUNG = 3,
    FLOWER = 4
};

public class Farm
{
    public Vector3Int loc;
    public int farmstate;

    public Farm(Vector3Int VectorInt, int farmstate)
    {
        this.loc = VectorInt;
        this.farmstate = farmstate;
    }

    public Farm(Vector3Int VectorInt)
    {
        this.loc = VectorInt;
        this.farmstate = (int)FARMSTATE.SEED;
    }
};
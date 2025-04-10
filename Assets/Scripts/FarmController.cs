using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmController : MonoBehaviour
{
    [SerializeField] private float minGrowthTime = 2f; // Minimum growth time
    [SerializeField] private float maxGrowthTime = 5f; // Maximum growth time

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private Tile[] tiles;

    private Dictionary<Vector3Int, Item.ItemType> seedTypeTracker;

    // Tiles managed by this FarmController
    [HideInInspector] public Vector3Int[] farmTiles;

    public Tilemap Map => map;

    private List<Farm> activeTiles;

    private Color targetColor = new Color(1.0f, 0.7f, 0.7f);

    void Start()
    {
        InitFarmTiles();
        activeTiles = new List<Farm>();
        seedTypeTracker = new Dictionary<Vector3Int, Item.ItemType>();
        Debug.Log($"FarmController '{name}' initialized with {farmTiles.Length} tiles.");
    }

    void Update()
    {
        FarmUpdate();
    }

    // Dynamically initialize farm tiles from the assigned Tilemap
    public void InitFarmTiles()
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (map.HasTile(pos))
            {
                tilePositions.Add(pos);
            }
        }

        farmTiles = tilePositions.ToArray();
        Debug.Log($"FarmController '{name}' initialized {farmTiles.Length} valid farm tiles.");
    }

    private void FarmUpdate()
    {
        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            Farm farm = activeTiles[i];
            farm.timer -= Time.deltaTime; 

            if (farm.timer > 0) continue; 

            // Reset the timer for the next growth stage
            farm.timer = UnityEngine.Random.Range(minGrowthTime, maxGrowthTime);

            if (farm.farmstate >= (int)FARMSTATE.FLOWER)
            {
                Debug.Log($"Tile at {farm.loc} in '{name}' has fully grown. Removing from active tiles.");
                activeTiles.RemoveAt(i);
                continue;
            }

            try
            {
                map.SetTile(farm.loc, tiles[++farm.farmstate + 4 * (int)farm.flower]);
                Debug.Log($"Tile at {farm.loc} in '{name}' updated to state {farm.farmstate}.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error updating tile at {farm.loc} in '{name}': {ex.Message}");
            }
        }
    }

    // Handles interaction with a specific tile
    public int InteractTile(Vector3Int spot, FLOWER flowerType)
    {
        Debug.Log($"InteractTile called for {spot} in FarmController '{name}'.");
        int status = 0;

        if (map.GetTile(spot) == tiles[(int)FARMSTATE.EMPTY])
        {
            Debug.Log($"Tile at {spot} in '{name}' is EMPTY. Planting SEED.");
            float initialTimer = UnityEngine.Random.Range(minGrowthTime, maxGrowthTime); // Random initial timer
            activeTiles.Add(new Farm(spot, (int)FARMSTATE.SEED, initialTimer, flowerType));
            map.SetTile(spot, tiles[(int)flowerType * 4 + (int)FARMSTATE.SEED]);
            status = 1;
            
        }
        else if (IsFlower(spot))
        {
            Debug.Log($"Tile at {spot} in '{name}' is FLOWER. Resetting to EMPTY.");
            map.SetTile(spot, tiles[(int)FARMSTATE.EMPTY]);
            status = 2;
        }
        else
        {
            Debug.Log($"Tile at {spot} in '{name}' is not interactable.");
        }
        return status;
    }

    public bool IsFlower(Vector3Int spot)
    {
        TileBase tile = map.GetTile(spot);
        if(tile == tiles[(int)FARMSTATE.FLOWER * 1] ||
           tile == tiles[(int)FARMSTATE.FLOWER * 2] ||
           tile == tiles[(int)FARMSTATE.FLOWER * 3] ||
           tile == tiles[(int)FARMSTATE.FLOWER * 4] ||
           tile == tiles[(int)FARMSTATE.FLOWER * 5])
        {
            return true;
        }
        return false;
    }

    public bool IsEmpty(Vector3Int spot){
        if(map.GetTile(spot) == tiles[(int)FARMSTATE.EMPTY])
        {
            return true;
        }
        return false;
    }

    public void setSeedTypeAtPos(Item.ItemType type, Vector3Int loc){
        seedTypeTracker[loc] = type;
    }

    public Item.ItemType getSeedTypeAtPos(Vector3Int loc, Boolean remove = false){
        Item.ItemType type = seedTypeTracker[loc];
        if (remove){
            seedTypeTracker.Remove(loc);
        }
        return type;
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

public enum FLOWER
{
    YELLOW = 0,
    BLUE = 1,
    BLACK = 2,
    PINK = 3,
    STAR = 4
}

public class Farm
{
    public Vector3Int loc;
    public int farmstate;
    public float timer;
    public FLOWER flower;

    public Farm(Vector3Int loc, int farmstate, float timer, FLOWER flower)
    {
        this.loc = loc;
        this.farmstate = farmstate;
        this.timer = timer;
        this.flower = flower;
    }

    public static Item.ItemType FlowerToItemType(FLOWER flower)
    {
        switch ((int)flower)
        {
            case (int)FLOWER.YELLOW:
                return Item.ItemType.SeedYellow;
            case (int)FLOWER.BLUE:
                return Item.ItemType.SeedBlue;
            case (int)FLOWER.BLACK:
                return Item.ItemType.SeedBlack;
            case (int)FLOWER.PINK:
                return Item.ItemType.SeedPink;
            case (int)FLOWER.STAR:
                return Item.ItemType.SeedStar;
            default:
                return Item.ItemType.Empty;
        }

    }

    public static FLOWER FlowerToItemType(Item.ItemType item)
    {
        switch ((int)item)
        {
            case (int)Item.ItemType.SeedYellow:
            case (int)Item.ItemType.FlowerYellow:
                return FLOWER.YELLOW;
            case (int)Item.ItemType.SeedBlue:
            case (int)Item.ItemType.FlowerBlue:
                return FLOWER.BLUE;
            case (int)Item.ItemType.SeedBlack:
            case (int)Item.ItemType.FlowerBlack:
                return FLOWER.BLACK;
            case (int)Item.ItemType.SeedPink:
            case (int)Item.ItemType.FlowerPink:
                return FLOWER.PINK;
            case (int)Item.ItemType.SeedStar:
            case (int)Item.ItemType.FlowerStar:
                return FLOWER.STAR;
            default:
                return FLOWER.YELLOW;//Might cause problems
        }

    }

    public static Color FlowerToColor(FLOWER flower)
    {
        switch ((int)flower)
        {
            case (int)FLOWER.YELLOW:
                return Color.yellow;
            case (int)FLOWER.BLUE:
                return Color.blue;
            case (int)FLOWER.BLACK:
                return Color.black;
            case (int)FLOWER.PINK:
                return Color.red;
            case (int)FLOWER.STAR:
                return Color.white;
            default:
                return Color.cyan;
        }

    }

    public static int FlowerHarvestRate(FLOWER flower)
    {
        switch ((int)flower)
        {
            case (int)FLOWER.YELLOW:
                return 3;
            case (int)FLOWER.BLUE:
                return 3;
            case (int)FLOWER.BLACK:
                return 3;
            case (int)FLOWER.PINK:
                return 2;
            case (int)FLOWER.STAR:
                return 2;
            default:
                return 0;
        }

    }
}

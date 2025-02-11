using System.Collections.Generic;
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

    // Tiles managed by this FarmController
    [HideInInspector] public Vector3Int[] farmTiles;

    public Tilemap Map => map;

    private List<Farm> activeTiles;

    private Color targetColor = new Color(1.0f, 0.7f, 0.7f);

    void Start()
    {
        InitFarmTiles();
        activeTiles = new List<Farm>();
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
            farm.timer = Random.Range(minGrowthTime, maxGrowthTime);

            if (farm.farmstate >= (int)FARMSTATE.FLOWER)
            {
                Debug.Log($"Tile at {farm.loc} in '{name}' has fully grown. Removing from active tiles.");
                activeTiles.RemoveAt(i);
                continue;
            }

            try
            {
                map.SetTile(farm.loc, tiles[++farm.farmstate]);
                Debug.Log($"Tile at {farm.loc} in '{name}' updated to state {farm.farmstate}.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error updating tile at {farm.loc} in '{name}': {ex.Message}");
            }
        }
    }

    // Handles interaction with a specific tile
    public int InteractTile(Vector3Int spot)
    {
        Debug.Log($"InteractTile called for {spot} in FarmController '{name}'.");
        int status = 0;

        if (map.GetTile(spot) == tiles[(int)FARMSTATE.EMPTY])
        {
            Debug.Log($"Tile at {spot} in '{name}' is EMPTY. Planting SEED.");
            float initialTimer = Random.Range(minGrowthTime, maxGrowthTime); // Random initial timer
            activeTiles.Add(new Farm(spot, (int)FARMSTATE.SEED, initialTimer));
            map.SetTile(spot, tiles[(int)FARMSTATE.SEED]);
            status = 1;
            
        }
        else if (map.GetTile(spot) == tiles[(int)FARMSTATE.FLOWER])
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
        if(map.GetTile(spot) == tiles[(int)FARMSTATE.FLOWER])
        {
            return true;
        }
        return false;
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
    public float timer;

    public Farm(Vector3Int loc, int farmstate, float timer)
    {
        this.loc = loc;
        this.farmstate = farmstate;
        this.timer = timer;
    }
}

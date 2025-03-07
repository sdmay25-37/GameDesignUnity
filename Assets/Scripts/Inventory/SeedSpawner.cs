using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 spawnAreaMin = new Vector2(-8, -22);
    private Vector2 spawnAreaMax = new Vector2(71, 23);

    private const string SEED1 = "Seed1";
    private const string SEED2 = "Seed2";
    private const string SEED3 = "Seed3";
    private const string SEED4 = "Seed4";
    private const string SEED5 = "Seed5";

    private Dictionary<String, (int, float)> seeds;
    void Start()
    {
        InitSeeds();
        SpawnSeeds();
    }

    private void InitSeeds(){
        seeds = new Dictionary<string, (int, float)>();
        seeds[SEED1] = (3, 0.5f);
        seeds[SEED2] = (3, 0.5f);
        seeds[SEED3] = (2, 0.4f);
        seeds[SEED4] = (5, 0.3f);
        seeds[SEED5] = (2, 0.1f);
    }

    void SpawnSeeds(){
        foreach (var seed in seeds){
            (int num, float prob) = seed.Value;
            for(int i = 0; i < num; i++){
                if(UnityEngine.Random.value < prob){
                    Vector3 randPos = new Vector3(
                        UnityEngine.Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                        UnityEngine.Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                        0
                    );
                    ItemObject.CreateItemObject(randPos, new Item{itemType=(Item.ItemType)Enum.Parse(typeof(Item.ItemType), seed.Key)});
                }
            }
        }
    }
}

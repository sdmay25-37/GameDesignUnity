using UnityEngine;

public class SpawnSeedStar : MonoBehaviour
{
    public Transform[] spawnPoints;

    public float probability = 0.3f;

    void Start()
    {
        SpawnSeeds();
    }

    void SpawnSeeds(){
        foreach (Transform point in spawnPoints){
            if(Random.value < probability){
                ItemObject.CreateItemObject(point.position, new Item{itemType=Item.ItemType.FlowerStar});
            }
        }
    }
}


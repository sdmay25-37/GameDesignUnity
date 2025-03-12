using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lantern, trap;

    public void SpawnLantern(Transform player)
    {
        GameObject light = Instantiate(lantern, player.position, Quaternion.identity);
        LightArea lightArea = light.GetComponent<LightArea>(); //Replace with serilized reference if too expensive
        lightArea.SetFarmer(gameObject.GetComponent<MainFarmer>());
        EnemyAI.AddLight(lightArea); //Check for enemy behaviour
    }

    public void SpawnTrap(Transform player)
    {
        Instantiate(trap, player.position, Quaternion.identity);
    }
}

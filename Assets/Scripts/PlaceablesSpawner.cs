using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceablesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lantern, trap;

    public void SpawnLantern(Transform player)
    {
        GameObject light = Instantiate(lantern, player.position, Quaternion.identity);
        EnemyAI.AddLight(light.GetComponent<LightArea>());
    }

    public void SpawnTrap(Transform player)
    {
        Instantiate(trap, player.position, Quaternion.identity);
    }
}

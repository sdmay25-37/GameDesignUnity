using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager manager;
    [SerializeField] EnemyAI[] enemies;
    [SerializeField] FastEnemyAI[] fEnemies;

    void OnAwake()
    {
        manager = this;
    }

    public static void AddLight(LightArea light)
    {
        foreach(EnemyAI ai in manager.enemies)
        {
            ai.AddLight(light);
        }

        foreach(FastEnemyAI ai in manager.fEnemies)
        {
            ai.AddLight(light);
        }
    }
}

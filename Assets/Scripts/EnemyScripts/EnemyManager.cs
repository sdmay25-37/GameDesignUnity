using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static List<EnemyAI> enemyList = new List<EnemyAI>();
    private static List<FastEnemyAI> fastEnemyList = new List<FastEnemyAI>();
    [SerializeField] EnemyAI[] enemies;// = new EnemyAI[2];
    [SerializeField] FastEnemyAI[] fEnemies;// = new FastEnemyAI[2];

    void Awake()
    {
        foreach (EnemyAI ai in enemies)
        {
            enemyList.Add(ai);
        }
        foreach(FastEnemyAI ai in fEnemies)
        {
            fastEnemyList.Add(ai);
        }
    }

    public static void AddLight(LightArea light)
    {
        foreach(EnemyAI ai in enemyList)
        {
            ai.AddLight(light);
        }

        foreach(FastEnemyAI ai in fastEnemyList)
        {
            ai.AddLight(light);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapSpawner : MonoBehaviour
{
    private Boolean isEmpty;
    private float exitTime;
    [SerializeField]
    private GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
        exitTime = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEntered");
        if(isEmpty && exitTime != -1)
        {
            SpawnChance();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TriggerExited = " + exitTime + " : " + Time.time);
        if (isEmpty && exitTime == -1)
        {
            exitTime = Time.time;
        }  
    }

    private void SpawnChance()
    {
        float odds = Random.Range(0f, 10f);
        if(odds < Time.time - exitTime)
        {
            Spawn();
            exitTime = -1;
            isEmpty = false;
        }
    }

    private void Spawn()
    {
        int enemyType = Random.Range(0, 2);
        Instantiate(enemies[enemyType], gameObject.transform.position, Quaternion.identity);
    }
}

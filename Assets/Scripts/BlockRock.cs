using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRock : MonoBehaviour
{
    public static bool destroyed;

    private void Start()
    {
        if(destroyed)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Found the rock");
            DemoGuyController.foundRock = true;
        }
    }
}

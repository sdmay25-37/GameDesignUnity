using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRenderOrder : MonoBehaviour
{
    private Transform mainfarmer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int offset = 0;

    private void Start()
    {
        mainfarmer = GameObject.Find("MainFarmer").GetComponent<Transform>();
    }

    private void Update()
    {
        if(mainfarmer.position.y > transform.position.y)
        {
            spriteRenderer.sortingOrder = 30 + offset;
        }
        else
        {
            spriteRenderer.sortingOrder = 10 + offset;
        }
    }
}

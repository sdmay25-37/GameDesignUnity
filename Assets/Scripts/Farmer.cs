using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Farmer : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector2 direction;
    [SerializeField]
    private Transform point;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private FarmController controller;
    private Boolean canClick;

    private void Start()
    {
        canClick = true;
    }


    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.forward);
        direction = Vector2.zero;
        if(Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        direction.Normalize();

        if (canClick && Input.GetMouseButton(0))
        {
            Action();
        }

        faceMouse();
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void Action()
    {
        Vector3Int pos = tilemap.WorldToCell(point.position);
        foreach(Vector3Int tile in controller.farmTiles)
        {
            if(tile.Equals(pos))
            {
                controller.InteractTile(pos);
                StartCoroutine(MouseCoolDown());
                return;
            }
        }
    }

    //Code from Danddx Tutorial on youtube: https://www.youtube.com/watch?v=_XdqA3xbP2A
    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;
    }

    IEnumerator MouseCoolDown()
    {
        canClick = false;
        point.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        point.gameObject.SetActive(true);
        canClick = true;
    }
}

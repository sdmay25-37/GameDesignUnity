using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FarmerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 10f;
    // public Sprite Farmer;

    private int extraLives = 0;

    private Inventory inventory;

    [SerializeField] private UI_Inventory uiInventory;

    public Vector2 direction;

    void Start()
    {
        // TODO: Implement the inventory logic
        // inventory = new Inventory();
        // uiInventory.SetInventory(inventory);
        // equipmentEffects();
    }

    // Update is called once per frame
    void Update()
    {

        movementAndOrientation();
    }

    // TODO: Add equipment effects
    private void equipmentEffects(){
        // extraLives++;
    }

    public int ExtraLives
    {
        get { return extraLives; }
        set { extraLives = value; } // Ensuring health doesn't go below zero
    }

    private void movementAndOrientation(){
        Vector2 move = Vector2.zero;

        bool update = false;
        int x = 0;
        int y = 0;

        if (Input.GetKey(KeyCode.W)){
            update = true;
            y += 1;
        } 
        if (Input.GetKey(KeyCode.A)){
            update = true;
            x -= 1;
        } 
        if (Input.GetKey(KeyCode.S)){
            update = true;
            y -= 1;
        } 
        if (Input.GetKey(KeyCode.D)){
            update = true;
            x += 1;
        } 
        if(update){
            move.x = x;
            move.y = y;
        }
        direction.x = x;
        direction.y = y;

        transform.rotation = Quaternion.Euler(0, 0, orientation(move));
        if (move.magnitude > 1){
            move = move.normalized;
        }
        transform.Translate(move * Time.deltaTime * moveSpeed, Space.World);
    }

    private float orientation(Vector2 calc){
        Vector3 mousePos = Input.mousePosition;
        Vector2 direction; 

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        direction = (mousePos - transform.position).normalized;
        if (Input.GetKey(KeyCode.Mouse0)){
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        return Mathf.Atan2(calc.y, calc.x) * Mathf.Rad2Deg;
    }
}

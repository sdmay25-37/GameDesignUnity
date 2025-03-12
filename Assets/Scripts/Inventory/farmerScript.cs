using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class FarmerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hat = true;

    private float multiplierSpeed = 6;

    private float regularSpeed = 3;

    public bool multiply = false;

    private Inventory inventory;

    private EquipmentSet equipmentSet;

    [SerializeField] private UI_Inventory uiInventory;

    public Vector2 direction;
    public GameObject inventoryContainer;

    private bool invent = false;

    void Start()
    {
        // TODO: Implement the inventory logic
        inventory = new Inventory();
        equipmentSet = new EquipmentSet();
        uiInventory.SetInventory(inventory);
        uiInventory.SetEquipmentSet(equipmentSet);
    }

    // Update is called once per frame
    void Update()
    {
        movementAndOrientation();
        displayInventory();
        uiInventory.SetFarmerPosition(gameObject.transform.position);
        equimentEffects();
    }

    private void displayInventory(){
        if (Input.GetKeyDown(KeyCode.Tab)){
            if(invent){
                invent = false;
                inventoryContainer.SetActive(false);
            }else{
                invent = true;
                inventoryContainer.SetActive(true);
            }
        } 
    }

    private void equimentEffects(){
        if(equipmentSet.GetEquipmentSetItem(0).itemType == Item.ItemType.Hat){
            hat = true;
        } else {
            hat = false;
        }
        if(equipmentSet.GetEquipmentSetItem(2).itemType == Item.ItemType.Shoes){
            multiply = true;
        }else { 
            multiply = false;
        }
    }

    public Inventory GetInventory {get {return inventory;}}

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
        if(multiply){
            transform.Translate(move * Time.deltaTime * multiplierSpeed, Space.World);
        }else{
            transform.Translate(move * Time.deltaTime * regularSpeed, Space.World);
        }
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

    public void Death(){
        if(hat == true){
            equipmentSet.UnequipItem(0, true);
        }else{
            Destroy(gameObject);
        }
    }

    public bool getLight(){
        return GetComponent<Light>();
    }

}



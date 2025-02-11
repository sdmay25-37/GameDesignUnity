using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class test_item : MonoBehaviour
{

    private Inventory inventory;

    [SerializeField] private Collider2D itemCollider;

    private void OnTriggerStay2D(Collider2D other) {
        //Debug.Log(other.tag);
        if(itemCollider.IsTouching(other)){
            switch (other.tag){
                case "Item":
                    if(Input.GetKey(KeyCode.E)){
                        if(inventory == null){
                            inventory = GetComponentInParent<MainFarmer>().GetInventory;
                        }
                        ItemObject itemObject = other.GetComponent<ItemObject>();
                        Debug.Log(inventory);
                        if(itemObject != null){
                            inventory.AddItem(itemObject.GetItem());
                            itemObject.DestroySelf();
                        }
                        pickUp(other.gameObject);
                    }       
                    break;
                default:
                    break;
            }
        }
    }

    private void pickUp(GameObject other){
        Debug.Log("Item picked up");
        Destroy(other);
    }
}

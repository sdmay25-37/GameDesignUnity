using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class test_item : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other) {
        switch (other.tag){
            case "Item":
                if(Input.GetKey(KeyCode.E)){
                    pickUp(other.gameObject);
                }       
                break;
            default:
                break;
        }
    }

    private void pickUp(GameObject other){
        Debug.Log("Item picked up");
        Destroy(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class interactionCollider : MonoBehaviour
{

    private FarmerScript farmer;

    private void Start()
    {
        farmer = transform.parent.GetComponent<FarmerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        interact(other.gameObject);
    }

    private void interact(GameObject other){
        
        switch (other.tag){
            case "Monster":
                Debug.Log("You died");
                death();
                break;
            default:
                break;
        }
    }

    private void death(){
        if(farmer.ExtraLives > 0){
            farmer.ExtraLives--;
        }else{
            Destroy(transform.parent.gameObject);
        }
    }
}
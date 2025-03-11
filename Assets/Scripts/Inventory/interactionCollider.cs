using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class interactionCollider : MonoBehaviour
{

    private MainFarmer farmer;
    [SerializeField] private Collider2D bodyCollider;

    private void Start()
    {
        farmer = transform.GetComponent<MainFarmer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        interact(other.gameObject);
    }

    private void interact(GameObject other){
        switch (other.tag){
            case "Monster":
                Debug.Log("You died");
                farmer.Death();
                break;
            default:
                break;
        }
    }
}
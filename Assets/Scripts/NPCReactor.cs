using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReactor : MonoBehaviour
{
    [SerializeField] private NPCController controller;

    public void Interact()
    {
        controller.Interaction();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.name.Equals("MainFarmer"))
        {
            Debug.Log("Not Farmer: " + other.gameObject.name);
            return;
        }
        Debug.Log("Exiting");
        controller.Exit();
    }
}

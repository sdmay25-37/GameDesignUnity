using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] protected Canvas popup;


    virtual public void Interaction()
    {
        
    }

    virtual public void Exit()
    {
        popup.gameObject.SetActive(false);
    }
}

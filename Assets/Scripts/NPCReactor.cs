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
}

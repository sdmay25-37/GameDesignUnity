using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour
{
    [SerializeField] private Boolean single_use;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrapReactorBase trapReactor = collision.GetComponent<TrapReactorBase>();

        if(trapReactor != null )
        {
            trapReactor.Trapped();
            if(single_use)
            {
                Destroy(this);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerTrapReactor : TrapReactorBase
{
    MainFarmer farmerscript;

    public void Start()
    {
        farmerscript = GetComponent<MainFarmer>();
        if(farmerscript == null )
        {
            Debug.LogError("FarmerReactor Missing Farmer Script on Gameobject: " + gameObject.name);
        }
    }
    // Start is called before the first frame update
    override public void Trapped()
    {
        StartCoroutine(farmerscript.Immobilize());
    }
}

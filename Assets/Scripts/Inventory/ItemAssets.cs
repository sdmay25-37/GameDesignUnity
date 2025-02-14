using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    
    public static ItemAssets Instance { get; private set;}

    private void Awake(){
        Instance = this;
    }

    public Transform pfItemObject;
    public Sprite gPaHat;
    public Sprite normalLantern;
    public Sprite coin;
    public Sprite shoes;
    public Sprite empty;
    public Sprite seed1;
    public Sprite seed2;
    public Sprite seed3;  
}

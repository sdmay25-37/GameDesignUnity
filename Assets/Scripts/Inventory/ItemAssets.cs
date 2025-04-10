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
    public Sprite trap;
    public Sprite coin;
    public Sprite shoes;
    public Sprite empty;
    public Sprite SeedYellow, SeedBlue, SeedBlack, SeedPink, SeedStar;
    public Sprite FlowerYellow, FlowerBlue, FlowerBlack, FlowerPink, FlowerStar;
}

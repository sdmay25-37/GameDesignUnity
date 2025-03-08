using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using System;

[Serializable]
public class Item {
    public enum ItemType {
        Seed1,
        Seed2,
        Seed3,
        Seed4,
        Seed5,
        Coin,
        Lantern,
        Hat,
        Shoes,
        Empty,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite(){
        switch (itemType){
            default:
                case ItemType.Coin: return ItemAssets.Instance.coin;
                case ItemType.Seed1: return ItemAssets.Instance.seed1;
                case ItemType.Seed2: return ItemAssets.Instance.seed2;
                case ItemType.Seed3: return ItemAssets.Instance.seed3;
                case ItemType.Seed4: return ItemAssets.Instance.seed4;
                case ItemType.Seed5: return ItemAssets.Instance.seed5;
                case ItemType.Lantern: return ItemAssets.Instance.normalLantern;
                case ItemType.Hat: return ItemAssets.Instance.gPaHat;
                case ItemType.Shoes: return ItemAssets.Instance.shoes;
                case ItemType.Empty: return ItemAssets.Instance.empty;
        }
    }

    public bool isStackable(){
        switch (itemType){
            case ItemType.Coin:
                return true;
            case ItemType.Lantern:
                return true;
            case ItemType.Seed1:
                return true;
            case ItemType.Seed2:
                return true;   
            case ItemType.Seed3:
                return true;
            case ItemType.Seed4:
                return true;
            case ItemType.Seed5:
                return true;
            default:
                return false;
        }
    }

    public bool isEquipment(){
        switch (itemType){
            case ItemType.Hat or ItemType.Shoes or ItemType.Lantern or ItemType.Seed1 or ItemType.Seed2 or ItemType.Seed3 or ItemType.Seed4 or ItemType.Seed5:
                return true;
            default:
                return false;
        }
    }
}

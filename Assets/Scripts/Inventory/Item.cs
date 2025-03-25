using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using System;

[Serializable]
public class Item {
    public enum ItemType {
        SeedYellow,
        SeedBlue,
        SeedBlack,
        SeedPink,
        SeedStar,
        Coin,
        Lantern,
        Trap,
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
                case ItemType.SeedYellow: return ItemAssets.Instance.seed1;
                case ItemType.SeedBlue: return ItemAssets.Instance.seed2;
                case ItemType.SeedBlack: return ItemAssets.Instance.seed3;
                case ItemType.SeedPink: return ItemAssets.Instance.seed4;
                case ItemType.SeedStar: return ItemAssets.Instance.seed5;
                case ItemType.Lantern: return ItemAssets.Instance.normalLantern;
                case ItemType.Trap: return ItemAssets.Instance.trap;
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
            case ItemType.Trap:
                return true;
            case ItemType.SeedYellow:
                return true;
            case ItemType.SeedBlue:
                return true;   
            case ItemType.SeedBlack:
                return true;
            case ItemType.SeedPink:
                return true;
            case ItemType.SeedStar:
                return true;
            default:
                return false;
        }
    }

    public bool isEquipment(){
        switch (itemType){
            case ItemType.Hat or ItemType.Shoes:
                return true;
            default:
                return false;
        }
    }
}

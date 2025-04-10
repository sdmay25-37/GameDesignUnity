using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using System;

[Serializable]
public class Item {
    public enum ItemType {
        SeedYellow, FlowerYellow,
        SeedBlue, FlowerBlue,
        SeedBlack, FlowerBlack,
        SeedPink, FlowerPink,
        SeedStar, FlowerStar,
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
                case ItemType.SeedYellow: return ItemAssets.Instance.SeedYellow;
                case ItemType.FlowerYellow: return ItemAssets.Instance.FlowerYellow;
                case ItemType.SeedBlue: return ItemAssets.Instance.SeedBlue;
                case ItemType.FlowerBlue: return ItemAssets.Instance.FlowerBlue;
                case ItemType.SeedBlack: return ItemAssets.Instance.SeedBlack;
                case ItemType.FlowerBlack: return ItemAssets.Instance.FlowerBlack;
                case ItemType.SeedPink: return ItemAssets.Instance.SeedPink;
                case ItemType.FlowerPink: return ItemAssets.Instance.FlowerPink;
                case ItemType.SeedStar: return ItemAssets.Instance.SeedStar;
                case ItemType.FlowerStar: return ItemAssets.Instance.FlowerStar;
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
    
    public Item handlFlower(){
        switch(itemType){
            case ItemType.FlowerYellow: return new Item{itemType=ItemType.SeedYellow, amount=3};
            case ItemType.FlowerBlue: return new Item{itemType=ItemType.SeedBlue, amount=3};
            case ItemType.FlowerBlack: return new Item{itemType=ItemType.SeedBlack, amount=3};
            case ItemType.FlowerPink: return new Item{itemType=ItemType.SeedPink, amount=2};
            case ItemType.FlowerStar: return new Item{itemType=ItemType.SeedBlue, amount=2};
            default: return null;
        }
    }
}

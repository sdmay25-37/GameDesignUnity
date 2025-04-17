using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
using System.Diagnostics.Tracing;

public class EquipmentSet {
    public EventHandler OnEquipmentListChanged;
    private Item[] equipmentSetList;

    public EquipmentSet(){
        equipmentSetList = MainManager.Instance.equipmentSetList;
        if (!MainManager.Instance.equipmentInit){
            MainManager.Instance.equipmentInit = true;
            equipmentSetList[0] = new Item {itemType = Item.ItemType.Empty};
            equipmentSetList[1] = new Item {itemType = Item.ItemType.Empty};
            equipmentSetList[2] = new Item {itemType = Item.ItemType.Empty};
        }
    }

    public Item.ItemType EquipItem(Item item, int index){
        Item current = GetEquipmentSetItem(index);
        UnequipItem(index);
        equipmentSetList[index] = item;
        if(item.itemType == Item.ItemType.Hat)
        {
            Farm.hatMod = 2;
        }
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
        return current.itemType;
    }

    public void UnequipItem(int index, bool destroy = false){
        // if(destroy){
        //     equipmentSetList[index].GameObject
        // }
        if (equipmentSetList[index].itemType == Item.ItemType.Hat)
        {
            Farm.hatMod = 0;
        }
        equipmentSetList[index] = new Item {itemType = Item.ItemType.Empty};
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetEquipmentSlot(Item item){
        switch (item.itemType){
            case Item.ItemType.Hat: return 0;
            case Item.ItemType.Lantern: return 1;
            case Item.ItemType.Shoes: return 2;
            default: return 1;
        }
    }

    public Item GetEquipmentSetItem(int index){
        return equipmentSetList[index];
    }

    public Item.ItemType hasSeeds(){
        Item holding = GetEquipmentSetItem(1);
        switch (holding.itemType){
            case Item.ItemType.SeedYellow: return Item.ItemType.SeedYellow;
            case Item.ItemType.SeedBlue: return Item.ItemType.SeedBlue;
            case Item.ItemType.SeedBlack: return Item.ItemType.SeedBlack;
            case Item.ItemType.SeedPink: return Item.ItemType.SeedPink;
            case Item.ItemType.SeedStar: return Item.ItemType.SeedStar;
            default: return Item.ItemType.Empty;
        }
    }
}

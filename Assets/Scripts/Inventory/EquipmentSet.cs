using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

public class EquipmentSet {
    public EventHandler OnEquipmentListChanged;
    private Item[] equipmentSetList;

    public EquipmentSet(){
        equipmentSetList = new Item[3];
        equipmentSetList[0] = new Item {itemType = Item.ItemType.Empty};
        equipmentSetList[1] = new Item {itemType = Item.ItemType.Empty};
        equipmentSetList[2] = new Item {itemType = Item.ItemType.Empty};
    }

    public void EquipItem(Item item, int index){
        equipmentSetList[index] = item;
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UnequipItem(int index, bool destroy = false){
        // if(destroy){
        //     equipmentSetList[index].GameObject
        // }
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
}

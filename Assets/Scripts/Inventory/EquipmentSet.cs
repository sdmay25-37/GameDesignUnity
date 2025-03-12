using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
using System.Diagnostics.Tracing;

public class EquipmentSet
{
    public EventHandler OnEquipmentListChanged;
    private Item[] equipmentSetList;

    public EquipmentSet()
    {
        equipmentSetList = MainManager.Instance.equipmentSetList;
        if (!MainManager.Instance.equipmentInit)
        {
            MainManager.Instance.equipmentInit = true;
            equipmentSetList[0] = new Item { itemType = Item.ItemType.Empty };
            equipmentSetList[1] = new Item { itemType = Item.ItemType.Empty };
            equipmentSetList[2] = new Item { itemType = Item.ItemType.Empty };
        }
    }

    public Item.ItemType EquipItem(Item item, int index)
    {
        Item current = GetEquipmentSetItem(index);
        UnequipItem(index);
        equipmentSetList[index] = item;
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
        return current.itemType;
    }

    public void UnequipItem(int index, bool destroy = false)
    {
        // if(destroy){
        //     equipmentSetList[index].GameObject
        // }
        equipmentSetList[index] = new Item { itemType = Item.ItemType.Empty };
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetEquipmentSlot(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Hat: return 0;
            case Item.ItemType.Lantern: return 1;
            case Item.ItemType.Shoes: return 2;
            default: return 1;
        }
    }

    public Item GetEquipmentSetItem(int index)
    {
        return equipmentSetList[index];
    }

    public Item.ItemType hasSeeds()
    {
        Item holding = GetEquipmentSetItem(1);
        switch (holding.itemType)
        {
            case Item.ItemType.Seed1: return Item.ItemType.Seed1;
            case Item.ItemType.Seed2: return Item.ItemType.Seed2;
            case Item.ItemType.Seed3: return Item.ItemType.Seed3;
            case Item.ItemType.Seed4: return Item.ItemType.Seed4;
            case Item.ItemType.Seed5: return Item.ItemType.Seed5;
            default: return Item.ItemType.Empty;
        }
    }
}
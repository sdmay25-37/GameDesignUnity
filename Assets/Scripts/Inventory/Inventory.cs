using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    public Inventory(){
        itemList = new List<Item>();

        AddItem(new Item {itemType = Item.ItemType.Seed1, amount = 4});
        AddItem(new Item {itemType = Item.ItemType.Coin, amount = 3});
        AddItem(new Item {itemType = Item.ItemType.Lantern, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.Hat, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.Shoes, amount = 1});
    }

    public void AddItem(Item item){
        if(item.isStackable()){
            bool alreadyExists = false;
            foreach (Item inventoryItem in itemList){
                if(inventoryItem.itemType == item.itemType){
                    inventoryItem.amount += item.amount;
                    alreadyExists = true;
                }
            }
            if(!alreadyExists){
                itemList.Add(item);
            }
        } else{
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item){
    if(item.isStackable()){
        Item itemInInventory = null;
        foreach (Item inventoryItem in itemList){
            if(inventoryItem.itemType == item.itemType){
                inventoryItem.amount -= 1;
                itemInInventory = inventoryItem;
            }
        }
        if(itemInInventory != null && itemInInventory.amount <= 0){
            itemList.Remove(itemInInventory);
        }
    } else{
        itemList.Remove(item);
    }
    OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // to be called when the user uses an item, not sure what that will look like yet
    public void useItem(Item item){

    }

    public List<Item> GetItemList(){
        return itemList;
    }

    public Boolean hasSeeds(Item item){
        foreach (Item inventoryItem in itemList){
            if(inventoryItem.itemType == item.itemType){
                return true;
            }
        }
        return false;
    }

}

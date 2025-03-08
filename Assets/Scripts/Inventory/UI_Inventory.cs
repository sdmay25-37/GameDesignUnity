using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory; 

    private EquipmentSet equipmentSet;
    // Potentially migrate to hosting transforms in respective classes
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Transform[] equipmentSlots = new Transform[3];
    private Vector3 position;

    private bool init = false;
    private bool setUpEquipmentClickHandlers = false;

    private void Awake(){
        locate();
    }

    public void SetFarmerPosition(Vector3 position){
        this.position = position;
    }

    private void locate(){
        if (!init){
            itemSlotContainer = GameObject.Find("itemSlotContainer").transform;
            itemSlotTemplate = GameObject.Find("itemSlotTemplate").transform;
            equipmentSlots[0] = GameObject.Find("hat").transform;
            equipmentSlots[1] = GameObject.Find("tool").transform;
            equipmentSlots[2] = GameObject.Find("shoes").transform;
            init = true;
        }
    }

    public void SetInventory(Inventory inventory){
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    public void SetEquipmentSet(EquipmentSet equipmentSet){
        this.equipmentSet = equipmentSet;
        equipmentSet.OnEquipmentListChanged += EquipmentSet_OnEquipmentSetListChanged;
        RefreshEquipmentItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void EquipmentSet_OnEquipmentSetListChanged(object sender, EventArgs e){
        RefreshEquipmentItems();
    }

    private void RefreshInventoryItems(){
        foreach (Transform child in itemSlotContainer){
            if(child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 55f;
        foreach (Item item in inventory.GetItemList()){
            if (!init) locate();
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            ClickHandler clickHandler = itemSlotRectTransform.gameObject.AddComponent<ClickHandler>();
            clickHandler.LeftClickFunc = () =>
            {
                // Use item
                if(item.isEquipment()){
                    Item.ItemType currentItemType = equipmentSet.EquipItem(item, equipmentSet.GetEquipmentSlot(item));
                    inventory.RemoveItem(item);
                    if (currentItemType != Item.ItemType.Empty){
                        inventory.AddItem(new Item {itemType=currentItemType, amount = 1});
                    }
                }
            };
            clickHandler.RightClickFunc = () =>
            {
                // Drop item
                Item temp = new Item { itemType = item.itemType, amount = item.amount};
                inventory.RemoveItem(item);
                Debug.Log(temp);
                ItemObject.DropItem(position, temp);
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image icon = itemSlotRectTransform.Find("itemIcon").GetComponent<Image>();
            icon.sprite = item.GetSprite();
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();
            if(item.amount > 1){
                uiText.SetText(item.amount.ToString());
            }else{
                uiText.SetText("");
            }
            x++;
            if (x > 4) {
                x = 0;
                y--;
            }
        }
    }

    private void RefreshEquipmentItems()
    {
        if (!init) locate();
        for(int i = 0; i < equipmentSlots.Length; i++){
            int temp = i;
            Transform slot = equipmentSlots[i];
            Item item = equipmentSet.GetEquipmentSetItem(i);
            if(!setUpEquipmentClickHandlers){
                RectTransform rectSlot = equipmentSlots[i].GetComponent<RectTransform>();

                ClickHandler clickHandler = rectSlot.gameObject.AddComponent<ClickHandler>();
                clickHandler.LeftClickFunc = () =>
                {
                    // Use item
                    // Maybe Display Info?
                    Debug.Log("Cliked an eSlot");
                };
                clickHandler.RightClickFunc = () =>
                {
                    // Unequip Item and add it back to the inventory
                    if(equipmentSet.GetEquipmentSetItem(temp).itemType != Item.ItemType.Empty){
                        Item transerItem = equipmentSet.GetEquipmentSetItem(temp);
                        inventory.AddItem(new Item{itemType = transerItem.itemType, amount=1});
                        equipmentSet.UnequipItem(temp);
                    }
                };
            }

            Image icon = slot.Find("itemIcon").GetComponent<Image>();
            icon.sprite = equipmentSet.GetEquipmentSetItem(i).GetSprite();
        }
        setUpEquipmentClickHandlers = true;
    }


}

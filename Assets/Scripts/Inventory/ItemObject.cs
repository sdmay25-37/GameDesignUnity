using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    public static ItemObject CreateItemObject(Vector3 position, Item item, int amount = 1){
        Transform transform = Instantiate(ItemAssets.Instance.pfItemObject, position, Quaternion.identity);
        ItemObject itemObject = transform.GetComponent<ItemObject>();
        itemObject.SetItem(item);
        item.amount = amount;
        return itemObject;
    }

    public static ItemObject DropItem(Vector3 dropPosition, Item item){
        ItemObject itemObject = CreateItemObject(dropPosition, item);
        return itemObject;
    }

    private Item item;

    private SpriteRenderer sprite;
    private void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item){
        this.item = item;
        sprite.sprite = this.item.GetSprite();
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }


}

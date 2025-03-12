using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : NPCController
{
    [SerializeField] private bool sellsTraps;
    [SerializeField] private int cost;
    private Item soldItem;
    private Item.ItemType costType;
    private string itemName, costName;
    private Inventory inv;
    private bool talking;
    [SerializeField] private TextMeshProUGUI dialog;

    private void Start()
    {
        if(sellsTraps)
        {
            soldItem = new Item {itemType = Item.ItemType.Trap, amount = 1};
            itemName = "trap";
            costName = "yellow seeds";
            costType = Item.ItemType.Seed1;
        }
        else
        {
            soldItem = new Item { itemType = Item.ItemType.Lantern, amount = 1 };
            itemName = "lantern";
            costName = "black seeds";
            costType = Item.ItemType.Seed1; //Change seed depending
        }
    }

    public override void Interaction()
    {
        if(talking)
        {
            return;
        }

        if(popup.gameObject.activeSelf)
        {
            popup.gameObject.SetActive(false);
            return;
        }

        if(CanAfford())
        {
            Trade();
        }
        else
        {
            NotEnoughMessage();
        }
    }

    private bool CanAfford()
    {
        if(inv == null)
        {
            inv = Inventory.GetInventory();
        }

        int itemCount = inv.GetItemCount(costType);

        if(itemCount >= cost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Trade()
    {
        for(int i = 0; i < cost; i++)
        {
            inv.RemoveItem(new Item { itemType = costType, amount = 1 });
        }

        inv.AddItem(new Item { itemType = soldItem.itemType, amount = soldItem.amount});
    }

    private void NotEnoughMessage()
    {
        popup.gameObject.SetActive(true);
        string message = "You are a few " + costName + " short, come back when you've got at least " + cost + " and I'll trade you for a " + itemName + ".";
        StartCoroutine(SwitchText(message));
    }

    private IEnumerator SwitchText(string text)
    {
        talking = true;
        for (int i = 1; i <= text.Length; i++)
        {
            dialog.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(0.05f);
            if (i != text.Length && IsPunctuation(text.Substring(i - 1, 1)))
            {
                yield return new WaitForSeconds(0.35f);
            }
        }
        talking = false;
    }

    private bool IsPunctuation(string character)
    {
        if (character == "." || character == "," || character == "!" || character == "?")
        {
            return true;
        }
        return false;
    }
}

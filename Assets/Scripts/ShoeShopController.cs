using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoeShopController : NPCController
{
    private static bool boughtShoes;
    private int cost = 20;
    private Item soldItem = new Item {itemType = Item.ItemType.Shoes, amount = 1};
    private Item.ItemType costType = Item.ItemType.SeedBlue;
    private Inventory inv;
    private bool talking;
    [SerializeField] private TextMeshProUGUI dialog;

    public override void Interaction()
    {
        if (talking)
        {
            return;
        }

        if (popup.gameObject.activeSelf)
        {
            popup.gameObject.SetActive(false);
            return;
        }

        if (boughtShoes)
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("You already have the shoes, you don't need another pair"));
        }
        else if(!CanAfford())
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("Your a few seeds short, bring me 20 blue seeds and I'll give you a pair of swift soles"));
        }
        else
        {
            Trade();
        }
    }
    private bool CanAfford()
    {
        if (inv == null)
        {
            inv = Inventory.GetInventory();
        }

        int itemCount = inv.GetItemCount(costType);

        if (itemCount >= cost)
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
        for (int i = 0; i < cost; i++)
        {
            inv.RemoveItem(new Item { itemType = costType, amount = 1 });
        }

        inv.AddItem(new Item { itemType = soldItem.itemType, amount = soldItem.amount });
        SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.shopSound);
        boughtShoes = true;
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

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HatShopController : NPCController
{
    private static bool boughtHat;
    private int cost = 15;
    private Item soldItem = new Item { itemType = Item.ItemType.Hat, amount = 1 };
    private Item.ItemType costType = Item.ItemType.SeedPink;
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

        if (boughtHat)
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("You can't wear two hats, I'm not sure why you would even want another one"));
        }
        else if (!CanAfford())
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("Your a few seeds short, bring me 15 pink seeds and I'll sell you this farmers hat. It'll improve the yields of your crop"));
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
        boughtHat = true;
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

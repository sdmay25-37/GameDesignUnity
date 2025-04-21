using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemoGuyController : NPCController
{
    public static bool foundRock, boughtDemo;
    private bool talking;
    private int cost = 10;
    [SerializeField] private TextMeshProUGUI dialog;
    [SerializeField] private float timeBetweenLetters = 0.04f;
    [SerializeField] private float timeBetweenPunctuation = 0.25f;
    private Item.ItemType costType = Item.ItemType.SeedStar;
    private Inventory inv;

    private void Start()
    {
        if(boughtDemo)
            Destroy(gameObject);
    }

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

        if(!foundRock)
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("Let me know if you find any rocks blocking your path, I can get rid of them for you"));
        }
        else if(boughtDemo)
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("I'll have that rock removed as soon as possible"));
        }
        else if (!CanAfford())
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("If you bring me 10 star seeds, I'll get rid of that pesky rock blocking your path"));
        }
        else
        {
            popup.gameObject.SetActive(true);
            StartCoroutine(SwitchText("I'm on it. That rock will be long gone the next time you enter the forest"));
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
        boughtDemo = true;
        BlockRock.destroyed = true;
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

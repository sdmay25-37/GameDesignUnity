using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
    public enum ItemType {
        Seed,
        Coin,
        Lantern,
    }

    public ItemType itemType;
    public int amount;

}

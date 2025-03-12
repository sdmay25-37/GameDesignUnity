using System.Collections.Generic;
using UnityEngine;

// Class used to maintain state for inventory and equipment when transitioning between scenes.
public class MainManager : MonoBehaviour
{

    public static MainManager Instance;

    public List<Item> itemList = new List<Item>();
    public Item[] equipmentSetList = new Item[3];

    public bool inventoryInit = false;
    public bool equipmentInit = false;

    public bool died = false;

    private void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}

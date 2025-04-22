using UnityEngine;

public class test_item : MonoBehaviour
{

    private Inventory inventory;

    [SerializeField] private Collider2D itemCollider;

    private void OnTriggerStay2D(Collider2D other) {
        //Debug.Log(other.tag);
        if(itemCollider.IsTouching(other)){
            switch (other.tag){
                case "Item":
                    if(Input.GetKey(KeyCode.Space)){
                        if(inventory == null){
                            inventory = GetComponentInParent<MainFarmer>().GetInventory;
                        }
                        ItemObject itemObject = other.GetComponent<ItemObject>();
                        if(itemObject != null){
                            inventory.AddItem(itemObject.GetItem());
                            MainFarmer.UnlockSeed(Farm.FlowerToItemType(itemObject.GetItem().itemType));
                            SoundManager.Instance.PlaySFX(SoundManager.Instance.sounds.harvestSound);
                            itemObject.DestroySelf();
                        }
                        pickUp(other.gameObject);
                    }       
                    break;
                default:
                    break;
            }
        }
    }

    private void pickUp(GameObject other){
        Debug.Log("Item picked up");
        Destroy(other);
    }
}

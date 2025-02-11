using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemWorldSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;

    private void Awake() {
        ItemObject.CreateItemObject(transform.position, item);
        Destroy(gameObject);
    }

}

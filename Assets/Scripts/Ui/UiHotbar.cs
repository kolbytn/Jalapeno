using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHotbar : MonoBehaviour
{

    ItemSlot[] slots;

    // Start is called before the first frame update
    void Start() {
        slots = GetComponentsInChildren<ItemSlot>();

        
    }

    public void SetInventory(Inventory inventory) {
        for(int i=0; i<inventory.Size(); i++) {
            Item item = inventory.ItemAt(i);
            if (item != null) {
                slots[i].SetItem(inventory.ItemAt(i));
            }
        }
    }

}

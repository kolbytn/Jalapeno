using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHotbar : MonoBehaviour
{

    ItemSlot[] slots;

    // Start is called before the first frame update
    void Start() {
        slots = GetComponentsInChildren<ItemSlot>();
        Debug.Log("chicken");

        foreach(ItemSlot slot in slots) {
            Debug.Log("slot");
            slot.SetIcon(WorldResources.BerriesIcon);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}

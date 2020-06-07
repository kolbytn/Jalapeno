// A glorified list of items
using System;
using UnityEngine;

public class Inventory : IEntity {

    int occupiedSlots;

    Item[] items;

    public Inventory(int size, Item[] StartItems = null) {
        items = new Item[size];
        
        if (StartItems != null) {
            foreach (Item item in StartItems) {
                AddItem(item);
            }
        }
    }

    // Loops through the list of items and stacks on duplicate item
    // If the item stack overflows, it will add to the next dup item, etc. 
    // If there is no dup item, it will occupy a new item slot
    // If there are no more item slots available, return the remaining item.
    // Otherwise return null
    public Item AddItem(Item new_item) {
        for (int i=0; i<items.Length; i++) {
            new_item = AddToSlot(new_item, i);
            if (new_item == null) {
                return null;
            }
        }
        return new_item;
    }

    // Adds an item to a given slot. If the slot is empty the new item will occupy it and return null.
    // If the slot is occupied by the same item type it will add its quanity to it.
    // If there is any left over the item's quantity will be updated and it will be returned. 
    // If the slot is occupied by a non duplicate return the new item unchanged.
    // tldr; adds to slot and returns whatever is left
    public Item AddToSlot(Item new_item, int slot) {
        Item old_item = items[slot];
        if (old_item == null) { // if the slot is empty occupy it
            items[slot] = new_item;
            return null;
        }
        if (new_item.GetType() == old_item.GetType()) { // if its a dup item
            int overflow = old_item.AddQuantity(new_item.Quantity); // add to the items quantity
            if(overflow > 0) { // if it has left over quanitity
                new_item.SetQuantity(overflow); // set the items quantity to the leftover value/return
                return new_item;
            }
            else {
                return null; // the item is fully added
            }
        }
        return new_item; // return the leftover values
    }


    // Sets the item at the given location to null
    public void Remove(int slot) {
        items[slot] = null;
    }


    // Loops through all the items and removes the ones that have a quantity lower than 1
    public void ClearEmptyItems() {
        for(int i=0; i<items.Length; i++) {
            if (items[i]!=null && items[i].Quantity <= 0) {
                Remove(i);
            }
        }
    }

    public Item ItemAt(int i) {
        return items[i];
    }

    public int Size() {
        return items.Length;
    }

    [Serializable]
    private struct InventoryInfo {

        public ItemInfo[] itemArray;

        [Serializable]
        public struct ItemInfo {

            public string Type;
            public string Info;
        }
    }

    public string ObjectToString() {
        InventoryInfo info;

        info.itemArray = new InventoryInfo.ItemInfo[items.Length];
        for (int i = 0; i < items.Length; i++) {

            if (items[i] != null) {
                InventoryInfo.ItemInfo itemInfo;
                itemInfo.Type = items[i].GetType().ToString();
                itemInfo.Info = items[i].ObjectToString();
                info.itemArray[i] = itemInfo;
            }
        }

        return JsonUtility.ToJson(info);
    }

    public IEntity ObjectFromString(string info) {
        InventoryInfo inventoryInfo = JsonUtility.FromJson<InventoryInfo>(info);

        // Set interactable objects
        items = new Item[inventoryInfo.itemArray.Length];
        for (int i = 0; i < inventoryInfo.itemArray.Length; i++) {

            if (items[i] != null) {
                object newObject = Activator.CreateInstance(null, inventoryInfo.itemArray[i].Type);
                Item newItem = (Item)newObject;
                newItem.ObjectFromString(inventoryInfo.itemArray[i].Info);
                items[i] = newItem;
            }
        }

        return this;
    }
}
// A glorified list of items
public class Inventory {

    readonly int totalSlots;
    readonly int occupiedSlots;

    Item[] items;

    public Inventory(int TotalSlots, Item[] StartItems = null) {
        this.totalSlots = TotalSlots;
        items = new Item[TotalSlots];
        
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
            // this should be unnecessary, but keeping just in case
            // if (item == null){
            //     break;
            // }
            // if(itemType == item.GetType()) {
            //     int overflow = item.addQuantity(new_item.Quantity);
            //     if(overflow > 0){
            //         item.setQuantity(overflow);
            //     }
            //     else {
            //         return null;
            //     }
            // }
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

    public void Remove() {
        // TODO: add item removal functionality
    }


}
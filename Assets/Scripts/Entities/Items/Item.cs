using UnityEngine;
// An item only exists in inventories. It can be used by the player. Items stack.
public abstract class Item : IEntity {

    protected int maxQuantity = 1;
    public int Quantity {
        get;
        protected set;
    }

    public Item(int Quantity) {
        // how to ensure quantity is smaller than max quantity? max quantity is overriden in the child class constructor methods
        this.Quantity = Quantity;
    }

    public abstract void Use(Character user);

    public int AddQuantity(int q) {
        if(Quantity + q <= maxQuantity) {
            Quantity += q;
            return 0;
        }
        int overflow = (Quantity+q) - maxQuantity;
        Quantity=maxQuantity;
        return overflow;
    }

    public void SetQuantity(int q) {
        if (q > maxQuantity) {
            Debug.LogError("Quantity was set to value larger than max quantity");
        }
        Quantity = q;
        if (q < 0) {
            Quantity = 0;
        }
    }

    // removes the value from the quantity. returns true if the item has 0 or less
    public bool Remove(int q) {
        Quantity -= q;
        return Quantity <= 0;
    }

    public virtual Sprite GetIconSprite() {
        return null;
    }

    public abstract string ObjectToString();

    public abstract IEntity ObjectFromString(string info);
}
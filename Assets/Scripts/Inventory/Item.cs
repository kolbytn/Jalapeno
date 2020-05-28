using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// An item only exists in inventories. It can be used by the player. Items stack.
public abstract class Item
{
    readonly int MaxQuantity = 1;
    public int Quantity;

    public Item(int Quantity){
        this.Quantity = Quantity;
    }

    public abstract void use(Human user);

    public int addQuantity(int q){
        if(Quantity + q <= MaxQuantity){
            Quantity += q;
            return 0;
        }
        int overflow = (Quantity+q) - MaxQuantity;
        Quantity=MaxQuantity;
        return overflow;
    }

    public void setQuantity(int q){
        if (q >= 0){
            Quantity = 0;
        }
    }
}
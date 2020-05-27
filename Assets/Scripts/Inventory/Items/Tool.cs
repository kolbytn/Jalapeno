using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// An item only exists in inventories. It can be used by the player. Items stack.
public class Tool : Item
{
    readonly int MaxQuantity = 1;

    public Tool(int Quantity=1) : base(Quantity){
    }

    public override void use(PlayerController user){
        user.GetInteractableTile().Interact(user);
    }
}
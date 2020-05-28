using System;
using UnityEngine;

// objects that occupy a grid location and can be interacted with by tools
public abstract class InteractableObject : WorldObject
{


    public abstract void Interact(Human user);
}
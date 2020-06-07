using UnityEngine;

public class Tool : Item {

    public Tool(int Quantity=1) : base(Quantity) {
        maxQuantity = 1;
    }

    public override void Use(Character user) {
        // if an item needs the interactable tile it will have to check if it's null
        if (user.GetInteractableTile() != null){
            user.GetInteractableTile().Interact(user);
        }
    }

    public override Sprite GetIconSprite() {
        return WorldResources.ShovelIcon;
    }

    public override string ObjectToString() {
        return "";
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }
}

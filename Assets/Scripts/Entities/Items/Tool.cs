using UnityEngine;

public class Tool : Item {

    public Tool(int Quantity=1) : base(Quantity) {
        maxQuantity = 1;
    }

    public override void use(Character user) {
        user.GetInteractableTile().Interact(user);
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

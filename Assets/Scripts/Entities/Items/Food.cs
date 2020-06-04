using UnityEngine;

public class Food : Item {

    public Food(int Quantity=1) : base(Quantity) {
        maxQuantity = 10;
    }

    public override void use(Character user) {
        user.GetInteractableTile().Interact(user);
    }

    public override Sprite GetIconSprite() {
        return WorldResources.BerriesIcon;
    }

    public override string ObjectToString() {
        return "";
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }
}
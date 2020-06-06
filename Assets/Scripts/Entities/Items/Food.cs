using UnityEngine;

public class Food : Item {

    public Food(int Quantity=1) : base(Quantity) {
        maxQuantity = 10;
    }

    public override void Use(Character user) {
        user.Eat(5);
        Quantity--;
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
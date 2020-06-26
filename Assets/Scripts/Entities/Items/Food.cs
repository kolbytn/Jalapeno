using UnityEngine;

public class Food : Item {

    public Food(int Quantity=1) : base(Quantity) {
    }

    public override void UseL(Character user) {
        user.Eat(Calories());
        Quantity--;
    }
    
    public override void UseR(Character user) {

    }

    public virtual int Calories() {
        return 10;
    }

    public override Sprite GetIconSprite() {
        return WorldResources.BerriesIcon;
    }

    public override int MaxQuantity() {
        return 10;
    }

    public override string ObjectToString() {
        return "";
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }
}
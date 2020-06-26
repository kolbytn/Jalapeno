using UnityEngine;

public class Berry : Food {

    public Berry(int Quantity=1) : base(Quantity) {
    }

    public override int Calories() {
        return 10;
    }

    public override Sprite GetIconSprite() {
        return WorldResources.BerriesIcon;
    }

    public override int MaxQuantity() {
        return 15;
    }
}
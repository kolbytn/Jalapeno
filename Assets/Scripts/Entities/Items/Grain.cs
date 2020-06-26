using UnityEngine;

public class Grain : Food {

    public Grain(int Quantity=1) : base(Quantity) {
    }

    public override int Calories() {
        return 1;
    }

    public override Sprite GetIconSprite() {
        return WorldResources.GrainIcon;
    }

    public override int MaxQuantity() {
        return 100;
    }
}
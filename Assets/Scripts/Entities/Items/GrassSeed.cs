using UnityEngine;

public class GrassSeed : Item {

    public GrassSeed(int Quantity=1) : base(Quantity) {
    }

    public override void UseL(Character user) {
        if (user.PlaceObject(WorldResources.Grass)) {
            Quantity--;
        }
    }

    public override void UseR(Character user) {
    }

    public override Sprite GetIconSprite() {
        return WorldResources.GrassSeedIcon;
    }

    public override int MaxQuantity() {
        return 100;
    }

    public override string ObjectToString() {
        return "";
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }
}
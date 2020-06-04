// An item only exists in inventories. It can be used by the player. Items stack.
public abstract class Item : IEntity {

    readonly int maxQuantity = 1;
    public int Quantity;

    public Item(int Quantity) {
        this.Quantity = Quantity;
    }

    public abstract void use(Character user);

    public int AddQuantity(int q) {
        if(Quantity + q <= maxQuantity) {
            Quantity += q;
            return 0;
        }
        int overflow = (Quantity+q) - maxQuantity;
        Quantity=maxQuantity;
        return overflow;
    }

    public void SetQuantity(int q) {
        if (q >= 0) {
            Quantity = 0;
        }
    }

    public abstract string ObjectToString();

    public abstract IEntity ObjectFromString(string info);
}
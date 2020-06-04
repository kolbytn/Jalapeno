// An item only exists in inventories. It can be used by the player. Items stack.
public class Tool : Item {

    public Tool(int Quantity=1) : base(Quantity) {}

    public override void use(Character user) {
        user.GetInteractableTile().Interact(user);
    }

    public override string ObjectToString() {
        return "";
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }
}

/**
Some design notes:

- the visual effects of different tools (highlighting tiles, etc) should be controlled by tools?
    also, some visual effects depend on whether you are the player or an npc
- should the player have two inventories? easy equip (shown at all times) and backpack (shown in menu)
- Items should have primary/secondary uses? eg chop, attack
- Some items should be composed of shared actions
    Ex swipe - tools and weapons should be able to swipe, but with different radiuses and damage outputs


Item base class ideas
Tool - necessary? What does it contain that Items don't?
Edible - has food count

**/

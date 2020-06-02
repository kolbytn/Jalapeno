// objects that occupy a grid location and can be interacted with by tools
public abstract class WorldObject : WorldEntity {

    public abstract void Interact(Character user);
}
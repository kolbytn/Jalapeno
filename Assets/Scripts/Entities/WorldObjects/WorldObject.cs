// objects that occupy a grid location and can be interacted with by tools
public abstract class WorldObject : WorldEntity {

    protected new void Start() {
        base.Start();
    }

    public abstract void Interact(Character user);
}
using UnityEngine;

public abstract class WorldEntity : MonoBehaviour, IEntity {

    public abstract string ObjectToString();

    public abstract IEntity ObjectFromString(string info);

    public float GetLocationX() {
        return transform.position.x;
    }

    public float GetLocationY() {
        return transform.position.y;
    }
}

using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public abstract string ObjectToString();

    public abstract WorldObject ObjectFromString(string info);

    public float GetLocationX()
    {
        return transform.position.x;
    }

    public float GetLocationY()
    {
        return transform.position.y;
    }
}

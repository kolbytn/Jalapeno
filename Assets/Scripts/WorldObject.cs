using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public abstract string ObjectToString();

    public abstract WorldObject ObjectFromString(string json);

    public int GetLocationX()
    {
        return (int)transform.position.x;
    }

    public int GetLocationY()
    {
        return (int)transform.position.y;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldObject : MonoBehaviour
{
    public abstract string ObjectToString();

    public abstract WorldObject ObjectFromString(string json);

    public int GetLocationX()
    {
        //  why int? I think this causes a glitch that truncates everything's position upon reloading
        // for instance bushes are not at the center of their grid location after reloading
        return (int)transform.position.x;
    }

    public int GetLocationY()
    {
        return (int)transform.position.y;
    }
}

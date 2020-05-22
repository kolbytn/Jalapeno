using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject
{
    int GetLocationX();

    int GetLocationY();

    string ObjectToString();

    IWorldObject ObjectFromString(string json);

    GameObject GetGameObject();
}

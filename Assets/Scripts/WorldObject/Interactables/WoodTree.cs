using System;
using UnityEngine;

public class WoodTree : InteractableObject
{
    [Serializable]
    private struct WoodTreeInfo
    {
    }

    public override void  Interact(PlayerController user)
    {
        Debug.Log("treeeee!!");
        
    }

    public override WorldObject ObjectFromString(string info)
    {
        WoodTreeInfo treeInfo = JsonUtility.FromJson<WoodTreeInfo>(info);

        return this;
    }

    public override string ObjectToString()
    {
        WoodTreeInfo treeInfo;

        return JsonUtility.ToJson(treeInfo);
    }
}

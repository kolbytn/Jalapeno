using System;
using UnityEngine;

public class WoodTree : WorldObject {

    [Serializable]
    private struct WoodTreeInfo {}

    public override void  Interact(Character user) {
        Debug.Log("treeeee!!");
    }

    public override IEntity ObjectFromString(string info) {
        WoodTreeInfo treeInfo = JsonUtility.FromJson<WoodTreeInfo>(info);
        return this;
    }

    public override string ObjectToString() {
        WoodTreeInfo treeInfo;
        return JsonUtility.ToJson(treeInfo);
    }
}

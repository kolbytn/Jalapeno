using System;
using UnityEngine;

public class BerryBush : WorldObject {

    public Animator Animator;

    bool hasBerries = true;
    float regrowCount = 0;
    readonly float regrowSpeed = 1;
    readonly float regrowMax = 50;

    protected new void Start() {
        base.Start();
        Animator.SetBool("HasBerries", hasBerries);
    }

    void Update() {

        if (!hasBerries) {

            regrowCount += regrowSpeed * Time.deltaTime;
            if (regrowCount > regrowMax) {

                Animator.SetBool("HasBerries", true);
                regrowCount = 0;
                hasBerries = true;
            }
        }
    }

    public override void  Interact(Character user) {
        if (hasBerries) {
            RemoveBerries();
            user.GiveItem(new Food(3));
        }
    }

    public void RemoveBerries() {
        if (hasBerries) {
            Animator.SetBool("HasBerries", false);
            hasBerries = false;
            regrowCount = 0;
        }
    }

    [Serializable]
    private struct BerryBushInfo {
        public bool hasBerries;
        public float regrowCount;
    }

    public override IEntity ObjectFromString(string info) 
        {
        BerryBushInfo bushInfo = JsonUtility.FromJson<BerryBushInfo>(info);
        hasBerries = bushInfo.hasBerries;
        regrowCount = bushInfo.regrowCount;

        Animator.SetBool("HasBerries", hasBerries);

        return this;
    }

    public override string ObjectToString() {

        BerryBushInfo bushInfo;
        bushInfo.hasBerries = hasBerries;
        bushInfo.regrowCount = regrowCount;

        return JsonUtility.ToJson(bushInfo);
    }
}

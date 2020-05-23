using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BerryBush : WorldObject
{
    public Animator animator;

    bool hasBerries = true;
    float regrowCount = 0;
    float regrowSpeed = 1;
    float regrowMax = 50;

    void Start()
    {
        animator.SetBool("HasBerries", hasBerries);
    }

    void Update()
    {
        if (!hasBerries)
        {
            regrowCount += regrowSpeed * Time.deltaTime;
            if (regrowCount > regrowMax)
            {
                animator.SetBool("HasBerries", true);
                regrowCount = 0;
                hasBerries = true;
            }
        }
    }

    public void RemoveBerries()
    {
        if (hasBerries)
        {
            animator.SetBool("HasBerries", false);
            hasBerries = false;
            regrowCount = 0;
        }
    }

    [Serializable]
    private struct BerryBushInfo
    {
        public bool hasBerries;
        public float regrowCount;
    }

    public override WorldObject ObjectFromString(string info)
    {
        BerryBushInfo bushInfo = JsonUtility.FromJson<BerryBushInfo>(info);
        hasBerries = bushInfo.hasBerries;
        regrowCount = bushInfo.regrowCount;

        animator.SetBool("HasBerries", hasBerries);

        return this;
    }

    public override string ObjectToString()
    {
        BerryBushInfo bushInfo;
        bushInfo.hasBerries = hasBerries;
        bushInfo.regrowCount = regrowCount;

        return JsonUtility.ToJson(bushInfo);
    }
}

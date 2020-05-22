using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BerryBush : WorldObject
{
    [Serializable]
    private struct BerryBushInfo
    {
        public int locx;
        public int locy;
        public bool hasBerries;
        public float regrowCount;
    }
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

    public override WorldObject ObjectFromString(string json)
    {
        BerryBushInfo bushInfo = JsonUtility.FromJson<BerryBushInfo>(json);
        hasBerries = bushInfo.hasBerries;
        regrowCount = bushInfo.regrowCount;

        if (hasBerries)
        {
            animator.SetBool("HasBerries", true);
        }
        else
        {
            animator.SetBool("HasBerries", false);
        }

        return this;
    }

    public override string ObjectToString()
    {
        BerryBushInfo bushInfo;
        bushInfo.locx = GetLocationX();
        bushInfo.locy = GetLocationY();
        bushInfo.hasBerries = hasBerries;
        bushInfo.regrowCount = regrowCount;

        return JsonUtility.ToJson(bushInfo);
    }
}

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

    public Sprite EmptyBushSprite;
    public Sprite FullBushSprite;

    bool hasBerries = true;
    float regrowCount = 0;
    float regrowSpeed = 1;
    float regrowMax = 50;

    void Start()
    {
    }

    void Update()
    {
        if (!hasBerries)
        {
            regrowCount += regrowSpeed * Time.deltaTime;
            if (regrowCount > regrowMax)
            {
                regrowCount = 0;
                hasBerries = true;
                GetComponent<SpriteRenderer>().sprite = FullBushSprite;
            }
        }
    }

    public void RemoveBerries()
    {
        if (hasBerries)
        {
            hasBerries = false;
            GetComponent<SpriteRenderer>().sprite = EmptyBushSprite;
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
            GetComponent<SpriteRenderer>().sprite = FullBushSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = EmptyBushSprite;
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

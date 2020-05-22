using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class BerryBush : MonoBehaviour, IWorldObject
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

    bool HasBerries = true;
    float RegrowCount = 0;
    float RegrowSpeed = 1;
    float RegrowMax = 50;

    void Start()
    {
    }

    void Update()
    {
        if (!HasBerries)
        {
            RegrowCount += RegrowSpeed * Time.deltaTime;
            if (RegrowCount > RegrowMax)
            {
                RegrowCount = 0;
                HasBerries = true;
                GetComponent<SpriteRenderer>().sprite = FullBushSprite;
            }
        }
    }

    public void RemoveBerries()
    {
        if (HasBerries)
        {
            HasBerries = false;
            GetComponent<SpriteRenderer>().sprite = EmptyBushSprite;
            RegrowCount = 0;
        }
    }

    public int GetLocationX()
    {
        return (int)transform.position.x;
    }

    public int GetLocationY()
    {
        return (int)transform.position.y;
    }

    public IWorldObject ObjectFromString(string json)
    {
        BerryBushInfo bushInfo = JsonUtility.FromJson<BerryBushInfo>(json);
        HasBerries = bushInfo.hasBerries;
        RegrowCount = bushInfo.regrowCount;

        if (HasBerries)
        {
            GetComponent<SpriteRenderer>().sprite = FullBushSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = EmptyBushSprite;
        }

        return this;
    }

    public string ObjectToString()
    {
        BerryBushInfo bushInfo;
        bushInfo.locx = GetLocationX();
        bushInfo.locy = GetLocationY();
        bushInfo.hasBerries = HasBerries;
        bushInfo.regrowCount = RegrowCount;

        return JsonUtility.ToJson(bushInfo);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}

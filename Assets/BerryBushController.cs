using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushController : MonoBehaviour
{
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
}

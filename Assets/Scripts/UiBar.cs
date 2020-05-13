using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    public Slider bar;

    public void initializeValues(float max, float cur)
    {
        bar.maxValue = max;
        bar.value = cur;
    }

    public void updateValue(float value)
    {
        bar.value = value;
    }
}

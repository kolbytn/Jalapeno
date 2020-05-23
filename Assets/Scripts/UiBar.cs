using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    public Slider bar;

    public void InitializeValues(float max, float cur)
    {
        bar.maxValue = max;
        bar.value = cur;
    }

    public void UpdateValue(float value)
    {
        bar.value = value;
    }
}

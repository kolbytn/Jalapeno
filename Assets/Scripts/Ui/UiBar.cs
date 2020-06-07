using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    public Slider Bar;

    public void InitializeValues(float max, float cur)
    {
        Bar.maxValue = max;
        Bar.value = cur;
    }

    public void UpdateValue(float value)
    {
        Bar.value = value;
    }
}

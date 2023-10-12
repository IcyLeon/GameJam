using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    public void SetMinandMaxValue(float min, float max)
    {
        slider.minValue = min;
        slider.maxValue = max;
    }

    // Update is called once per frame
    public void UpdateTime(float Time)
    {
        slider.value = Time;
    }
}

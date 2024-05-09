using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerSliderHelper : MonoBehaviour
{
    private Slider slider;
    [SerializeField]private TextMeshProUGUI timerText;
    public int index;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = GameManager.instance.timers[index].minMaxTimeInMinutes.x;
        slider.maxValue = GameManager.instance.timers[index].minMaxTimeInMinutes.y;
        UpdateText();
    }
    private void UpdateText()
    {
        var textValues = GameManager.instance.timers[index];
        timerText.text = textValues.displayText +  " : " + textValues.timeInMinutes;
    }
    public void ChangeTimerValue()
    {
        GameManager.instance.timers[index].timeInMinutes = slider.value;
        GameManager.instance.ChangeTimers();
        UpdateText();
    }
}

using UnityEngine;
using TMPro;
using System;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text valueText;

    private void UpdateValue()
    {
        valueText.text = GameManager.Instance.Score + "\n"
        + ConvertTime(GameManager.Instance.Time) + "\n"
        + ConvertSpeed(Lander.Instance.GetSpeedX()) + "\n"
        + ConvertSpeed(Lander.Instance.GetSpeedY()) + "\n"
        + Lander.Instance.GetFuelAmount().ToString("F2");
    }

    private float ConvertSpeed(float speed)
    {
        return Mathf.Round(speed * 10f);
    }

    private string ConvertTime(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        // return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void Update()
    {
        UpdateValue();
    }
}

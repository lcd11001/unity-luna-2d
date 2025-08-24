using UnityEngine;
using TMPro;
using System;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text valueText;
    [SerializeField]
    private GameObject arrowUp;
    [SerializeField]
    private GameObject arrowDown;
    [SerializeField]
    private GameObject arrowLeft;
    [SerializeField]
    private GameObject arrowRight;

    private void Awake()
    {
        arrowUp.SetActive(false);
        arrowDown.SetActive(false);
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
    }

    private void UpdateValue()
    {
        valueText.text = GameManager.Instance.Score + "\n"
        + ConvertTime(GameManager.Instance.Time) + "\n"
        + ConvertSpeed(Lander.Instance.GetSpeedX()) + "\n"
        + ConvertSpeed(Lander.Instance.GetSpeedY()) + "\n"
        + Lander.Instance.GetFuelAmount().ToString("F2");
    }

    private void UpdateArrow()
    {
        float epsilon = 0.1f;

        arrowUp.SetActive(Lander.Instance.GetSpeedY() > epsilon);
        arrowDown.SetActive(Lander.Instance.GetSpeedY() < -epsilon);
        arrowLeft.SetActive(Lander.Instance.GetSpeedX() < -epsilon);
        arrowRight.SetActive(Lander.Instance.GetSpeedX() > epsilon);
    }

    private float ConvertSpeed(float speed)
    {
        return Mathf.Abs(Mathf.Round(speed * 10f));
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
        UpdateArrow();
    }
}

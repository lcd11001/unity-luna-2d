using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
    [SerializeField]
    private Image fuelBar;

    // Tween colors from 0 to 100%
    private Color[] fuelBarColors = new Color[]
    {
        Color.red,
        Color.yellow,
        Color.green
    };

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
        + ConvertSpeed(Lander.Instance.GetSpeedY());
    }

    private void UpdateArrow()
    {
        float epsilon = 0.1f;

        arrowUp.SetActive(Lander.Instance.GetSpeedY() > epsilon);
        arrowDown.SetActive(Lander.Instance.GetSpeedY() < -epsilon);
        arrowLeft.SetActive(Lander.Instance.GetSpeedX() < -epsilon);
        arrowRight.SetActive(Lander.Instance.GetSpeedX() > epsilon);
    }

    private void UpdateFuelBar()
    {
        if (fuelBar != null)
        {
            float fuelAmount = Lander.Instance.GetFuelAmountNormalized();
            fuelBar.fillAmount = fuelAmount;

            if (fuelBarColors == null || fuelBarColors.Length == 0)
            {
                return; // No colors to use
            }

            if (fuelBarColors.Length == 1)
            {
                fuelBar.color = fuelBarColors[0];
                return;
            }

            // Calculate the position in the color array
            float colorPosition = fuelAmount * (fuelBarColors.Length - 1);

            // Determine the two colors to lerp between
            int index1 = Mathf.Clamp(Mathf.FloorToInt(colorPosition), 0, fuelBarColors.Length - 2);
            int index2 = index1 + 1;

            // Determine the lerp factor
            float lerpFactor = colorPosition - index1;

            fuelBar.color = Color.Lerp(fuelBarColors[index1], fuelBarColors[index2], lerpFactor);
        }
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
        UpdateFuelBar();
    }
}

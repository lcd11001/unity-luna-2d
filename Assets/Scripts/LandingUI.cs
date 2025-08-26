using TMPro;
using UnityEngine;

public class LandingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text statusText;

    void Awake()
    {
        panel.SetActive(false);
    }

    public void OnLandingEvent(OnLandingEvent landingEvent)
    {
        panel.SetActive(true);

        if (landingEvent.type == LandingType.Success)
        {
            titleText.text = "SUCCESSFUL LANDING!";

        }
        else
        {
            titleText.text = "<color=#ff0000>CRASH!</color>";
        }

        statusText.text = $"{ConvertSpeed(landingEvent.landingSpeed)}\n" +
                          $"{ConvertDotVector(landingEvent.landingDotVector)}\n" +
                          $"x{landingEvent.scoreMultiplier}\n" +
                          $"{landingEvent.score}";
    }

    private float ConvertSpeed(float speed)
    {
        return Mathf.Abs(Mathf.Round(speed * 10f));
    }

    private float ConvertDotVector(float dotVectorMagnitude)
    {
        return Mathf.Abs(Mathf.Round(dotVectorMagnitude * 100f));
    }

}

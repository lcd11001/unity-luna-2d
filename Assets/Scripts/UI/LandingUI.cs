using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text statusText;
    [SerializeField]
    private TMP_Text buttonText;
    [SerializeField]
    private Button restartButton;

    private Action onRestartAction;

    void Awake()
    {
        panel.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    void Update()
    {
        if (GameInput.Instance.IsMenuConfirmPressed() && panel.activeSelf)
        {
            OnRestartButtonClicked();
        }
    }

    private void OnRestartButtonClicked()
    {
        onRestartAction?.Invoke();
    }

    public void OnLandingEvent(OnLandingEvent landingEvent)
    {
        panel.SetActive(true);

        if (landingEvent.type == LandingType.Success)
        {
            titleText.text = "SUCCESSFUL LANDING!";
            buttonText.text = "CONTINUE";
            onRestartAction = GameManager.Instance.GoToNextLevel;
        }
        else
        {
            titleText.text = "<color=#ff0000>CRASH!</color>";
            buttonText.text = "RETRY";
            onRestartAction = GameManager.Instance.RetryLevel;
        }

        statusText.text = $"{ConvertSpeed(landingEvent.landingSpeed)}\n" +
                          $"{ConvertAngle(landingEvent.landingAngle)}\n" +
                          $"x{landingEvent.scoreMultiplier}\n" +
                          $"{landingEvent.score}";
    }

    private float ConvertSpeed(float speed)
    {
        return Mathf.Abs(Mathf.Round(speed * 10f));
    }

    private float ConvertAngle(float angle)
    {
        // return Mathf.Abs(Mathf.Round(angle * 100f));
        return Mathf.Round(angle);
    }

}

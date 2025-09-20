using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MenuUIBase
{
    [SerializeField] private Button buttonResume;

    private void Awake()
    {
        buttonResume.onClick.AddListener(OnResumeClicked);
    }

    protected override void Start()
    {
        InitializeButtons(buttonResume);
        base.Start();

        Hide();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed -= GameManager_OnGameResumed;
        }
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void OnResumeClicked()
    {
        Debug.Log("Resume button clicked");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}

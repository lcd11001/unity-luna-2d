using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MenuUIBase
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    protected override void Start()
    {
        InitializeButtons(mainMenuButton, quitButton);
        base.Start();

        if (GameManager.Instance != null)
        {
            scoreText.text = $"FINAL SCORE: {GameManager.Instance.TotalScore}";
        }
    }

    private void OnMainMenuClicked()
    {
        Debug.Log("MainMenu button clicked");
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadMainMenuScene();
        }
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MenuUIBase
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonQuit;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnPlayClicked);
        buttonQuit.onClick.AddListener(OnQuitClicked);
    }

    protected override void Start()
    {
        InitializeButtons(buttonPlay, buttonQuit);
        base.Start();
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        if (GameManager.Instance != null)
        {
            // GameManager.Instance.ResetLevel();
            Destroy(GameManager.Instance.gameObject);
        }

        Time.timeScale = 1f;

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadGameScene();
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

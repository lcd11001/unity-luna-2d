using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MenuUIBase
{
    [SerializeField] private Button buttonResume;
    [SerializeField] private Button buttonMainMenu;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderSFXVolume;

    private void Awake()
    {
        buttonResume.onClick.AddListener(OnResumeClicked);
        buttonMainMenu.onClick.AddListener(OnMainMenuClicked);

        sliderMusicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
        sliderSFXVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    protected override void Start()
    {
        InitializeButtons(buttonResume, buttonMainMenu);
        base.Start();

        Hide();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        }

        if (MusicManager.Instance != null)
        {
            // due to slider value range 0..10
            sliderMusicVolume.value = MusicManager.Instance.Volume;
        }

        if (SoundManager.Instance != null)
        {
            // due to slider value range 0..10
            sliderSFXVolume.value = SoundManager.Instance.Volume;
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

    private void OnMainMenuClicked()
    {
        Debug.Log("MainMenu button clicked");
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadMainMenuScene();
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

    private void OnMusicVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            int intValue = Mathf.RoundToInt(value / sliderMusicVolume.maxValue * MusicManager.MaxVolume);
            MusicManager.Instance.ChangeVolume(intValue);
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            int intValue = Mathf.RoundToInt(value / sliderSFXVolume.maxValue * SoundManager.MaxVolume);
            SoundManager.Instance.ChangeVolume(intValue);
        }
    }
}

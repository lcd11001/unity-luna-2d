using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button buttonResume;

    private void Awake()
    {
        buttonResume.onClick.AddListener(OnResumeClicked);
    }

    private void Start()
    {
        gameObject.SetActive(false);
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
            GameManager.Instance.OnGameResumed -= GameManager_OnGameResumed;
        }
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void GameManager_OnGameResumed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void OnResumeClicked()
    {
        GameManager.Instance.ResumeGame();
    }
}

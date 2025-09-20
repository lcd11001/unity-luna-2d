using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonQuit;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(OnPlayClicked);
        buttonQuit.onClick.AddListener(OnQuitClicked);
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        SceneLoader.Instance.LoadGameScene();
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

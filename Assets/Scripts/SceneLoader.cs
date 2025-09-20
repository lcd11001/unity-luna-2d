#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

#if UNITY_EDITOR
    [SerializeField] private SceneAsset gameScene;
    [SerializeField] private SceneAsset mainMenuScene;
    [SerializeField] private SceneAsset gameOverScene;
#endif

    [SerializeField, HideInInspector] private string gameSceneName;
    [SerializeField, HideInInspector] private string mainMenuSceneName;
    [SerializeField, HideInInspector] private string gameOverSceneName;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (gameScene != null)
        {
            gameSceneName = gameScene.name;
        }
        if (mainMenuScene != null)
        {
            mainMenuSceneName = mainMenuScene.name;
        }
        if (gameOverScene != null)
        {
            gameOverSceneName = gameOverScene.name;
        }
#endif
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public AsyncOperation LoadGameScene()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            return SceneManager.LoadSceneAsync(gameSceneName);
        }
        return null;
    }

    public AsyncOperation LoadMainMenuScene()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            return SceneManager.LoadSceneAsync(mainMenuSceneName);
        }
        return null;
    }

    public AsyncOperation LoadGameOverScene()
    {
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            return SceneManager.LoadSceneAsync(gameOverSceneName);
        }
        return null;
    }
}

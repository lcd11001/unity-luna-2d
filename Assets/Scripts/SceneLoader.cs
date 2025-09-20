using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private SceneAsset gameScene;
    [SerializeField] private SceneAsset mainMenuScene;

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
        if (gameScene != null)
        {
            return SceneManager.LoadSceneAsync(gameScene.name);
        }
        return null;
    }

    public AsyncOperation LoadMainMenuScene()
    {
        if (mainMenuScene != null)
        {
            return SceneManager.LoadSceneAsync(mainMenuScene.name);
        }
        return null;
    }
}

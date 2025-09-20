using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField]
    public int Score { get; private set; }

    [field: SerializeField]
    public float FlyingTime { get; private set; }

    [field: SerializeField]
    public int CurrentLevel { get; private set; }

    [SerializeField]
    private List<GameLevel> gameLevels;

    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    public static GameManager Instance { get; private set; }

    private bool isGameActive = false;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("GameManager started");
        LoadCurrentLevel();
        ResetParams();

        GameInput.Instance.OnMenuPaused += OnMenuPausedHandler;
    }

    private void OnMenuPausedHandler(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Debug.LogWarning("Application focus changed: " + hasFocus + ", isGameActive: " + isGameActive);
        if (!hasFocus && isGameActive)
        {
            PauseGame();
        }
    }

    private void Update()
    {
        // Should use event instead, to avoid direct reference
        // if (Lander.Instance == null || Lander.Instance.GetState() != Lander.State.Flying)
        // {
        //     return;
        // }
        if (isGameActive)
        {
            FlyingTime += UnityEngine.Time.deltaTime;
        }
    }

    public void HandleLandedSuccessful(OnLandingEvent landingEvent)
    {
        AddScore(landingEvent.score);
    }

    public void HandleCoinPickedUp(int amount)
    {
        AddScore(amount);
    }

    public void HandleLanderStateChanged(Lander.State state)
    {
        isGameActive = state == Lander.State.Flying;

        if (state != Lander.State.Waiting)
        {
            if (cinemachineCamera != null && Lander.Instance != null)
            {
                cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
                CinemachineCameraZoom2D.Instance.SetTargetZoom(10f);
                CinemachineCameraZoom2D.Instance.SetDeadZoneSize(0.2f, 0.2f);
            }
        }
    }

    private void AddScore(int amount)
    {
        Score += amount;
        Debug.Log("Score added: " + amount + ", new total: " + Score);
    }

    private void LoadCurrentLevel()
    {
        bool levelFound = false;
        // Load the current level data
        if (gameLevels != null && gameLevels.Count > 0)
        {
            foreach (var level in gameLevels)
            {
                if (level.LevelNumber == CurrentLevel)
                {
                    levelFound = true;
                    Debug.Log("Loading level: " + CurrentLevel);
                    // Instantiate the level prefab
                    var newLevel = Instantiate(level, Vector3.zero, Quaternion.identity);

                    // Position the player at the spawn point
                    if (Lander.Instance != null && newLevel.PlayerSpawnPoint != null)
                    {
                        Lander.Instance.transform.position = newLevel.PlayerSpawnPoint.position;
                    }

                    if (cinemachineCamera != null)
                    {
                        cinemachineCamera.Target.TrackingTarget = newLevel.TargetCamera;
                        CinemachineCameraZoom2D.Instance.SetTargetZoom(newLevel.CameraZoomSize);
                        CinemachineCameraZoom2D.Instance.SetDeadZoneSize(0, 0);
                    }
                    break;
                }
            }
        }

        if (!levelFound)
        {
            Debug.LogWarning("Level not found: " + CurrentLevel + ". Restarting from level 1.");
            CurrentLevel = 1;
            LoadCurrentLevel();
        }
    }

    private void ResetParams()
    {
        Score = 0;
        FlyingTime = 0f;
        isGameActive = false;

        // the reference to the Cinemachine camera is missing after reload the scene
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        CinemachineCameraZoom2D.Instance.SetVitualCamera(cinemachineCamera);
    }

    public void GoToNextLevel()
    {
        CurrentLevel++;
        Debug.Log("Going to next level: " + CurrentLevel);
        StartCoroutine(LoadGame());
    }

    public void RetryLevel()
    {
        Debug.Log("Retrying level");
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        var asyncLoad = SceneLoader.Instance.LoadGameScene();
        yield return new WaitUntil(() => asyncLoad.isDone);
        ResetParams();
        LoadCurrentLevel();
    }

    public void TogglePauseGame()
    {
        if (isGameActive)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGameActive = false;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGameActive = true;
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }
}
